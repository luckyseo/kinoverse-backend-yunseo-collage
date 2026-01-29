using Collage.Backend.Induction.Starter.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Options;
using System.Text.Json;
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
    [Route("api/[controller]")] //api/movies
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _moviesService;

        public MoviesController(IMovieService moviesService)
        {
            _moviesService = moviesService;
        }

        [HttpGet("/{genre}")]
        [ProducesResponseType(typeof(IEnumerable<MovieSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> GetMoviesByGenre(string genre)
        {
            //need to validate genre input
            var response = await _moviesService.GetMoviesByGenreAsync(Genre.GetGenreId(genre));
            return Ok(response);
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(typeof(MovieDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] // if TMDb cannot find the movie ID
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> GetMovieById(int movieId)
        {
            var response = await _moviesService.GetMovieDetailsByIdAsync(movieId);
            if(response == null)
            {
                throw new KeyNotFoundException($"TMDb returned 404 for movieId {movieId}");
            }
            return Ok(response);
        }
        [HttpGet("{movieId}/recommendations")]
        [ProducesResponseType(typeof(IEnumerable<MovieSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> GetRecommendations(int movieId)
        {
            var response = await _moviesService.GetMoviesByIdAndRecommendationsAsync(movieId);
            return Ok(response);   
        }

        [HttpPost("{movieId}/emotions")]
        [ProducesResponseType(typeof(TagEmotionResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // for invalid emotion 
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> AddEmotion([FromRoute] string movieId, [FromBody] TagEmotionRequestDto request)
        {
            var validEmotions = Enum.GetNames(typeof(EmotionType)).ToList();
            //need to validate available emotions
            if(!validEmotions.Contains(request.Emotion, StringComparer.OrdinalIgnoreCase))
            {
                throw new Exceptions.InvalidEmotionException($"{request.Emotion} is not a valid emotion. Allowed: {string.Join(", ", validEmotions)}"
        );
            }
            var response = await _moviesService.AddEmotionToMovieAsync(Convert.ToInt32(movieId), request);
            return Ok(response);
        }

    }
}
