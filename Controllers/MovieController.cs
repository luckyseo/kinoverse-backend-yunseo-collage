using Collage.Backend.Induction.Starter.Services;
using Microsoft.AspNetCore.Mvc;
using Clients;
using Models;
using DTOs;

namespace Collage.Backend.Induction.Starter.Controllers
{
    /*
    Validation and guard behaviour must be applied to all of the following (and any more that you have in your induction project):
    GET /api/movies/scifi - movie summaries
    GET /api/movies/{movieId} - return movie details
    GET /api/movies/{movieId}/recommendations - return movie summaries
    POST /api/movies/{movieId}/emotions

    ref: https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0
    return type: https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-10.0
    db: https://github.com/jellyfin/TMDbLib?tab=readme-ov-file
    API ref: https://developer.themoviedb.org/reference/changes-movie-list
    */
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _moviesService;

        public MoviesController(IMovieService moviesService)
        {
            _moviesService = moviesService;
        }

        [HttpGet("scifi")]
        [ProducesResponseType(typeof(IEnumerable<MovieSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMoviesByGenre(string genre)
        {
            var response = await _moviesService.GetMoviesByGenreAsync(Genre.GetGenreId(genre));
            return Ok(response);
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(typeof(MovieDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            var response = await _moviesService.GetMovieDetailsByIdAsync(movieId);
            return Ok(response);
        }
        [HttpGet("{movieId}/recommendations")]
        [ProducesResponseType(typeof(IEnumerable<MovieSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRecommendations(int movieId)
        {
            var response = await _moviesService.GetMoviesByIdAndRecommendationsAsync(movieId);
            return Ok(response);   
        }

        [HttpPost("{movieId}/emotions")]
        public async Task<IActionResult> AddEmotion(int movieId, [FromBody] TagEmotionRequestDto request)
        {
            var response = await _moviesService.AddEmotionToMovieAsync(movieId, request);
            return Ok(response);
        }

    }
}
