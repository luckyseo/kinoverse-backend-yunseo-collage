using Models;
using Models.Tmdb;
using Microsoft.Extensions.Options;

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

        public async Task<TmdbDiscoverResponse> GetMoviesByGenreAsync(int genreId)
        {
            var response = await _httpClient.GetAsync(
                $"discover/movie?with_genres={genreId}&api_key={_options.ApiKey}"
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TmdbDiscoverResponse>()
                ?? throw new InvalidOperationException("TMDb response was empty");
        }

        public async Task<TmdbMovieDetailResponse> GetMovieDetailsByIdAsync(int movieId)
        {
            var response = await _httpClient.GetAsync(
                $"movie/{movieId}?api_key={_options.ApiKey}"
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TmdbMovieDetailResponse>()
                ?? throw new InvalidOperationException("TMDb response was empty");
        }

        public async Task<TmdbRecommendationResponse> GetMovieRecommendationsAsync(int movieId)
        {
            var response = await _httpClient.GetAsync(
                $"movie/{movieId}/recommendations?api_key={_options.ApiKey}"
            );

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<TmdbRecommendationResponse>()
                ?? throw new InvalidOperationException("TMDb response was empty");
        }
    }
}
