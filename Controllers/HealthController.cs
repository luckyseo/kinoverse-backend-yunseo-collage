using Collage.Backend.Induction.Starter.Services;
using Microsoft.AspNetCore.Mvc;

namespace Collage.Backend.Induction.Starter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IHealthService _healthService;

        public HealthController(IHealthService healthService)
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
