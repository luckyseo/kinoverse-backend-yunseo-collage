using Collage.Backend.Induction.Starter.Models;

namespace Collage.Backend.Induction.Starter.Services
{
    public class HealthService : IHealthService
    {
        public HealthResponse GetHealth()
        {
            return new HealthResponse();
        }
    }
}
