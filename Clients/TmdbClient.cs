using Models;
using Models.Tmdb;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace Clients
{
    public class TmdbClient
    {
        private readonly HttpClient _httpClient;
        private readonly TmdbOptions _options;

        public TmdbClient(HttpClient httpClient, IOptions<TmdbOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }
        private async Task<T> GetAsync<T>(string url, string errorContext)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{url}&api_key={_options.ApiKey}");

                // Handle 404 (Requirement: Movie not found)
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException($"TMDb returned 404 for {errorContext}");
                }

                // Handle 500/502 (Requirement: TMDb is down / fails)
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<T>() 
                    ?? throw new Exception("TMDb returned an empty response.");
            }
            catch (HttpRequestException)
            {
                // This triggers the 502/503 mapping in your Middleware
                throw new Exception("TMDb is down / fails");
            }
        }
        public async Task<TmdbDiscoverResponse> GetMoviesByGenreAsync(int genreId)
        {
            return await GetAsync<TmdbDiscoverResponse>(
                $"{_options.BaseUrl}discover/movie?with_genres={genreId}"
                ,"Get Genre"
            );
        }

        public async Task<TmdbMovieDetailResponse> GetMovieDetailsByIdAsync(int movieId)
        {
            return await GetAsync<TmdbMovieDetailResponse>(
                $"{_options.BaseUrl}movie/{movieId}?",
                "Get Movie Details"
            );
        }

        public async Task<TmdbDiscoverResponse> GetMovieRecommendationsAsync(int movieId)
        {
            return await GetAsync<TmdbDiscoverResponse>(
                $"{_options.BaseUrl}movie/{movieId}/recommendations",
                $"Get Movie Recommendations from {movieId}"
            );
        }
    }
}
