using Collage.Backend.Induction.Starter.Models;

namespace Collage.Backend.Induction.Starter.Services
{
    public interface IHealthService
    {
        HealthResponse GetHealth();
    }
}