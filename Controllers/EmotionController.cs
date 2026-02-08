using System.Collections.Concurrent;
using Collage.Backend.Induction.Starter.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
//review
// BC: Controllers look neatly implemented. clear routing and response types. Good job on validation for genre and emotion types.
// Strictly for endpoints, logic is done inside services with minimal logic in controllers which is good practice.
//
namespace Collage.Backend.Induction.Starter.Controllers
{
    /*
    Validation and guard behaviour must be applied to all of the following (and any more that you have in your induction project):

    GET /api/emotions
    */
    [ApiController]
    [Route("api/[controller]")]
    public class EmotionsController : ControllerBase
    {
        private readonly IEmotionService _emotionsService;
        private readonly IMovieService _moviesService;

        public EmotionsController(IEmotionService emotionsService)
        {
            _emotionsService = emotionsService; 
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EmotionType>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSupportedEmotionTypes()
        {
            var response = await _emotionsService.GetEmotionTypesAsync();
            return Ok(response);
        }


    }
}
