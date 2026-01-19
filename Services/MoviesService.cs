using Clients;
using System.Net.Http.Headers;
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using Microsoft.Extensions.Caching.Memory;
using Models;
using DTOs;

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

        public async Task<MovieSummaryDto> GetMoviesByGenreAsync(string genre)
        {
            //https://api.themoviedb.org/3/genre/movie/list
            //https://api.themoviedb.org/3/search/movie 

            // Implementation to get movies by genre using TMDbLib
            int genreId = ResolveGenreId(genre);
            var movies = await _client.GetMoviesByGenreAsync(genreId);
            // Filter movies by genre logic here
            return new MovieSummaryDto{ Movies = movies.Results };
        }

        public async Task<MovieDetailDto> GetMovieDetailsByIdAsync(int movieId)
        {
            var movie = await _client.GetMovieDetailsByIdAsync(movieId);
            return new MovieDetailDto { Movie = movie };
        }
        public async Task<MovieSummaryDto> GetMoviesByIdAndRecommendationsAsync(int movieId)
        {
            var recommendations = await _client.GetMovieRecommendationsAsync(movieId);
            return new MovieSummaryDto { Movies = recommendations.Results };
        }

        public async Task<TagEmotionResponseDto> AddEmotionToMovieAsync(TagEmotionRequestDto request)
        {
            // Implementation to add emotion to a movie
            return await Task.FromResult(new TagEmotionResponseDto { Success = true });
        }
    }
}