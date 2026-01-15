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
    GET /api/emotions
    */
    [ApiController]
    [Route("api/[controller]")]
    public class EmotionsController : ControllerBase
    {
        private readonly IHealthService _healthService;

        public EmotionsController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            var response = _healthService.GetHealth();
            return Ok(response);
        }
       
        
    }
}
