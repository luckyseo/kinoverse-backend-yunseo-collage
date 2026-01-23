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
namespace Collage.Backend.Induction.Starter.Services
{
    public class MoviesService :IMovieService
    {
        // Define movie-related service methods here
        private readonly TmdbClient _client;
        private readonly IMemoryCache _cache;

        private static readonly ConcurrentDictionary<int, MovieEmotionState> _movieEmotionStates = new ConcurrentDictionary<int, MovieEmotionState>();
        private static EmotionStateDto EmotionForMovieDetail(int movieId)
        {
            var movieEmotionState = _movieEmotionStates.GetOrAdd(movieId, new MovieEmotionState());

            if(movieEmotionState.EmotionCounts.Values.All(value => value == 0))
            {
                return new EmotionStateDto
                            {
                                Counts = movieEmotionState.EmotionCounts.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                                UserEmotion = null // This will be set per user in the controller
                            };
            }
            else
            {
                var Max = movieEmotionState.EmotionCounts.Values.Max(); //index of max value
                var KeyOfMaxValue = movieEmotionState.EmotionCounts.FirstOrDefault(x => x.Value == Max).Key;

                return new EmotionStateDto
                {
                    Counts = movieEmotionState.EmotionCounts.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                    UserEmotion = KeyOfMaxValue // Top Emotion
                };
            }
            
        }
        public MoviesService(TmdbClient tmdbClient, IMemoryCache memoryCache)
        {
            _client = tmdbClient;
            _cache = memoryCache;
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
                    new EmotionSummary 
                    { 
                        TopEmotions = new List<TopEmotion>() 
                    }
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
                    new EmotionSummary 
                    { 
                        TopEmotions = new List<TopEmotion>() 
                    }
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
            if(!Enum.TryParse<EmotionType>(request.Emotion, true, out var ValidEmotion)){
                throw new ArgumentException("Invalid emotion type");
            }
            //if user does not exist in the dictionary, add new entry
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
            string cacheKey = $"movies:{movieId}";
            _cache.Remove(cacheKey);
            // Implementation to add emotion to a movie
            return await Task.FromResult(response);
        }
    }
}