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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMoviesByGenre()
        {
            var response = await _moviesService.GetMoviesByGenreAsync("scifi");
            return Ok(response);
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            var response = await _moviesService.GetMovieByIdAsync(movieId);
            return Ok(response);
        }
        [HttpGet("{movieId}/recommendations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecommendations(int movieId)
        {
            //var response = _healthService.GetMoviesHealth();
            //return Ok(response);  
             return Ok();  
        }

        [HttpPost("{movieId}/emotions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddEmotion(int movieId)
        {
             return Ok();
        }

    }
}
