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
            const string cacheKey = "movies:scifi";
            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<MovieSummaryDto> cached))
                return cached;
            var tmdbWrapper = await _client.GetMoviesByGenreAsync(genreId);
            if (tmdbWrapper?.Results == null)
            {
                return new List<MovieSummaryDto>(); // Return empty list instead of null
            }
            var movies = tmdbWrapper.Results
                .Select(MapToSummaryDto)
                .ToList();
            _cache.Set(cacheKey, movies, TimeSpan.FromMinutes(10));
            return movies;
        }
        private static MovieSummaryDto MapToSummaryDto(TmdbMovieItem raw)
        {
            // Parse the date string safely
            int year = 0;
            if (DateTime.TryParse(raw.ReleaseDate, out var date))
            {
                year = date.Year;
            }

            // Construct the full URL
            var fullPosterUrl = string.IsNullOrEmpty(raw.PosterPath) 
                ? "" // Handle missing images
                : $"https://image.tmdb.org/t/p/w500{raw.PosterPath}";

            return new MovieSummaryDto //replace by addung contructor
            {
                Id = raw.Id,
                Title = raw.Title,
                Overview = raw.Overview,
                ReleaseYear = year,
                PosterUrl = fullPosterUrl,
                EmotionSummary = new EmotionSummary 
                { 
                    TopEmotions = new List<TopEmotion>() 
                }
            };
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
                                    tmdbRaw.ReleaseDate != null && DateTime.TryParse(tmdbRaw.ReleaseDate, out var date) ? date.Year : 0,
                                    string.IsNullOrEmpty(tmdbRaw.PosterPath) ? "" : $"https://image.tmdb.org/t/p/w500{tmdbRaw.PosterPath}",
                                    tmdbRaw.Genres.Select(g => g.Id).ToList(),
                                    tmdbRaw.Runtime ?? 0,
                                    new EmotionStateDto()
                                );
            _cache.Set(cacheKey, movie, TimeSpan.FromMinutes(10));
            return movie;
        }
        // public async Task<MovieSummaryDto> GetMoviesByIdAndRecommendationsAsync(int movieId)
        // {
        //     string cacheKey = $"movie:{movieId}:recommendations";

        //     var recommendations = await _client.GetMovieRecommendationsAsync(movieId);
        //     return new MovieSummaryDto { Movies = recommendations.Results };
        // }

        // public async Task<TagEmotionResponseDto> AddEmotionToMovieAsync(TagEmotionRequestDto request)
        // {
        //     // Implementation to add emotion to a movie
        //     return await Task.FromResult(new TagEmotionResponseDto { Success = true });
        // }
    }
}