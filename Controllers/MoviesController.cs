using Collage.Backend.Induction.Starter.Services;
using Microsoft.AspNetCore.Mvc;

namespace Collage.Backend.Induction.Starter.Controllers
{
    /*
    Validation and guard behaviour must be applied to all of the following (and any more that you have in your induction project):
    GET /api/movies/scifi
    GET /api/movies/{movieId}
    GET /api/movies/{movieId}/recommendations
    POST /api/movies/{movieId}/emotions

    ref: https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-10.0
    return type: https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-10.0
    */
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;

        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMoviesHealth()
        {
           return Ok();
        }

        [HttpGet("scifi")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMoviesHealth()
        {
             return Ok();  
        }

        [HttpGet("{movieId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMoviesHealth()
        {
             return Ok();   
        }
        [HttpGet("{movieId}/recommendations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMoviesHealth()
        {
            //var response = _healthService.GetMoviesHealth();
            //return Ok(response);  
             return Ok();  
        }

        [HttpPut("{movieId}/emotions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMoviesHealth()
        {
             return Ok();
        }

    }
}
