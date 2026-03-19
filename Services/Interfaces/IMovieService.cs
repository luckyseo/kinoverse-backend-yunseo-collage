
using Models;
using System.Threading.Tasks;
using DTOs;
using System.Collections.Concurrent;
//review
// BC: Interface functions are defined well according to the required endpoints
// it is fit for purpose and clear in its intent
namespace Collage.Backend.Induction.Starter.Services
{
//     GET /api/movies/scifi - movie summaries
//     GET /api/movies/{movieId} - return movie details
//     GET /api/movies/{movieId}/recommendations - return movie summaries
//     POST /api/movies/{movieId}/emotions
    public interface IMovieService
    {
        // Define movie-related service methods here
        Task<IEnumerable<MovieSummaryDto>> GetMoviesByGenreAsync(int genreId);
        Task<MovieDetailDto> GetMovieDetailsByIdAsync(int movieId);
        Task<IEnumerable<MovieSummaryDto>> GetMoviesByIdAndRecommendationsAsync(int movieId);
        Task<TagEmotionResponseDto> AddEmotionToMovieAsync(int movieId, TagEmotionRequestDto request);
        Task<IDictionary<string, EmotionType?>> GetUserEmotionsForMovieAsync(int movieId);
    }
}