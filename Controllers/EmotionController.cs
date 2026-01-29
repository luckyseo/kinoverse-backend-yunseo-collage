using Collage.Backend.Induction.Starter.Services;
using Microsoft.AspNetCore.Mvc;
using Models;

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
