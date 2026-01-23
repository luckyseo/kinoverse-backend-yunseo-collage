using Clients;
using System.Net.Http.Headers;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using Microsoft.Extensions.Caching.Memory;
using Models;
using DTOs;
using Models.Tmdb;

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
            _cache.Set(cacheKey, movies, TimeSpan.FromMinutes(10));
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
                                    new EmotionStateDto()
                                );
            _cache.Set(cacheKey, movie, TimeSpan.FromMinutes(10));
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
            _cache.Set(cacheKey, movies, TimeSpan.FromMinutes(10));
            return movies;
        }

        public async Task<TagEmotionResponseDto> AddEmotionToMovieAsync(int movieId, TagEmotionRequestDto request)
        {
            // Implementation to add emotion to a movie
            return await Task.FromResult(new TagEmotionResponseDto());
        }
    }
}