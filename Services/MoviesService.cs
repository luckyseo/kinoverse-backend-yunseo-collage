using Clients;
using System.Net.Http.Headers;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using Microsoft.Extensions.Caching.Memory;
using Models;
using DTOs;
using Models.Tmdb;
using System.Collections.Concurrent;
using Sprache;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Routing.Constraints;
//review
namespace Collage.Backend.Induction.Starter.Services
{
    public class MoviesService :IMovieService
    {
        // Define movie-related service methods here
        private readonly TmdbClient _client;
        private readonly IMemoryCache _cache;

        public MoviesService(TmdbClient tmdbClient, IMemoryCache memoryCache)
        {
            _client = tmdbClient;
            _cache = memoryCache;
        }

        private static readonly ConcurrentDictionary<int, MovieEmotionState> _movieEmotionStates = new ConcurrentDictionary<int, MovieEmotionState>();
        private static EmotionStateDto EmotionForMovieDetail(int movieId)
        {
            var movieEmotionState = _movieEmotionStates.GetOrAdd(movieId, new MovieEmotionState());
            EmotionType? UserEmotion;
        
            var Max = movieEmotionState.EmotionCounts.Values.Max(); //index of max value
            var KeyOfMaxValue = movieEmotionState.EmotionCounts.FirstOrDefault(x => x.Value == Max).Key;
            if(Max == 0)
            {
                UserEmotion = null;
            }
            else
            {
                UserEmotion = KeyOfMaxValue;
            }
            return new EmotionStateDto
                {
                    Counts = movieEmotionState.EmotionCounts.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                    UserEmotion = UserEmotion // Top Emotion
                };
        }
   
        private static EmotionSummary EmotionForMovieSummary(int movieId)
        {
            var movieEmotionState = _movieEmotionStates.GetOrAdd(movieId, new MovieEmotionState());
            var TopEmotions = movieEmotionState.EmotionCounts
                                        .OrderByDescending(kvp => kvp.Value) 
                                        .ThenBy(kvp => kvp.Key)              
                                        .Take(3)                          
                                        .Select(kvp => new TopEmotion        
                                        { 
                                            Emotion = kvp.Key, 
                                            Count = kvp.Value 
                                        })
                                        .ToList();

            EmotionType? UserEmotion;
        
            if(TopEmotions[0].Count == 0)
            {
                UserEmotion = null;
            }
            else
            {
                UserEmotion = TopEmotions[0].Emotion;
            }
    
            return new EmotionSummary
                        {        
                            TopEmotions = TopEmotions,
                            UserEmotion = UserEmotion // No user-specific emotion in summary
                        };
            }

        public async Task<IEnumerable<MovieSummaryDto>> GetMoviesByGenreAsync(int genreId)
        {
            //https://api.themoviedb.org/3/genre/movie/list
            //https://api.themoviedb.org/3/search/movie 

            // Implementation to get movies by genre using TMDbLib
            string cacheKey = $"movies:genre:{genreId}";
            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<MovieSummaryDto> cached))
                return cached;

            var tmdbWrapper = await _client.GetMoviesByGenreAsync(genreId);
            if (tmdbWrapper?.Results == null)
            {
                return new List<MovieSummaryDto>(); // Return empty list instead of null
            }
            var movies = tmdbWrapper.Results
                .Select(raw => new MovieSummaryDto(
                    raw.Id,
                    raw.Title,
                    raw.Overview,
                    raw.ReleaseDate, //will be parsed in base class
                    raw.PosterPath,
                    EmotionForMovieSummary(raw.Id)
                ))
                .ToList();
            _cache.Set(cacheKey, movies, TimeSpan.FromMinutes(15));
            return movies;
        }


        public async Task<MovieDetailDto> GetMovieDetailsByIdAsync(int movieId)
        {
            string cacheKey = $"movies:{movieId}";

            if (_cache.TryGetValue(cacheKey, out MovieDetailDto cached))
            {
                return cached;
            }
            var tmdbRaw = await _client.GetMovieDetailsByIdAsync(movieId);        
            if (tmdbRaw == null)
            {
                return new MovieDetailDto(); 
            }
            var movie = new MovieDetailDto(
                                    tmdbRaw.Id, 
                                    tmdbRaw.Title, 
                                    tmdbRaw.Overview,
                                    tmdbRaw.ReleaseDate, //will be parsed in base class
                                    tmdbRaw.PosterPath,
                                    tmdbRaw.Genres.Select(g => g.Id).ToList(),
                                    tmdbRaw.Runtime ?? 0,
                                    EmotionForMovieDetail(movieId)
                                );
            _cache.Set(cacheKey, movie, TimeSpan.FromMinutes(20));
            return movie;
        }
        public async Task<IEnumerable<MovieSummaryDto>> GetMoviesByIdAndRecommendationsAsync(int movieId)
        {
                
            string cacheKey = $"movie:{movieId}:recommendations";
            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<MovieSummaryDto> cached))
                return cached;

            var recommendations = await _client.GetMovieRecommendationsAsync(movieId);
            if (recommendations?.Results == null)
            {
                return new List<MovieSummaryDto>(); // Return empty list instead of null
            }
            var movies = recommendations.Results
                .Select(raw => new MovieSummaryDto(
                    raw.Id,
                    raw.Title,
                    raw.Overview,
                    raw.ReleaseDate, 
                    raw.PosterPath,
                    EmotionForMovieSummary(raw.Id)
                ))
                .ToList();
            _cache.Set(cacheKey, movies, TimeSpan.FromMinutes(20));
            return movies;
        }

        public async Task<TagEmotionResponseDto> AddEmotionToMovieAsync(int movieId, TagEmotionRequestDto request)
        {

            var emotionState = _movieEmotionStates.GetOrAdd(movieId, new MovieEmotionState());//get existing or create new
            var userId = request.UserId;
            //validate whether the emotion exists
            EmotionType ValidEmotion = (Enum.Parse<EmotionType>(request.Emotion, true));
            //if user does not exist in the dictionary, add new entr>y
            if (!emotionState.UserEmotionsByUserId.ContainsKey(userId))
            {
                emotionState.UserEmotionsByUserId[userId] = null;
            }

            if (emotionState.UserEmotionsByUserId.TryGetValue(userId, out EmotionType? existingEmotion)) //check if user has existing emotion
            {
                if (existingEmotion.HasValue && existingEmotion.Value == ValidEmotion) //user add same emotion again
                {
                    emotionState.EmotionCounts[existingEmotion.Value]--; //decrement count of existing emotion
                    emotionState.UserEmotionsByUserId[userId] = null; //remove user's emotion
                }
                else if(existingEmotion.HasValue && existingEmotion.Value != ValidEmotion) //user change to different emotion
                {
                    emotionState.EmotionCounts[existingEmotion.Value]--; //decrement count of existing emotion
                    emotionState.EmotionCounts[ValidEmotion]++; //increment count of changed emotion
                    emotionState.UserEmotionsByUserId[userId] = ValidEmotion;
                }
                else //existing emotion is null, user is adding new emotion
                {
                    emotionState.EmotionCounts[ValidEmotion]++; //increment count of changed emotion
                    emotionState.UserEmotionsByUserId[userId] = ValidEmotion;
                }
            }
            TagEmotionResponseDto response = new TagEmotionResponseDto
            {
                MovieId = movieId.ToString(),
                EmotionState = new EmotionStateDto
                {
                    Counts = emotionState.EmotionCounts.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                    UserEmotion = emotionState.UserEmotionsByUserId[userId]
                }
            };

            //get genreIds from current movieId -> update cache accordingly
            string cache_movieID = $"movies:{movieId}";

            var tmdbRaw = await _client.GetMovieDetailsByIdAsync(movieId);  
            List<int> genreIds = tmdbRaw.Genres.Select(g => g.Id).ToList();
            foreach (var genreId in genreIds)
            {
                var cacheKey = $"movies:genre:{genreId}";
                _cache.Remove(cacheKey);
            }
            
            string cache_recommendation = $"movie:{movieId}:recommendations";
            _cache.Remove(cache_recommendation);
            _cache.Remove(cache_movieID);
            // Implementation to add emotion to a movie
            return await Task.FromResult(response);
        }
    
        public Task<IDictionary<string, EmotionType?>> GetUserEmotionsForMovieAsync(int movieId)
        {
            if (_movieEmotionStates.TryGetValue(movieId, out var emotionState))
            {
                return Task.FromResult<IDictionary<string, EmotionType?>>(emotionState.UserEmotionsByUserId);
            }
            return Task.FromResult<IDictionary<string, EmotionType?>>(new Dictionary<string, EmotionType?>());
        }
    }
}