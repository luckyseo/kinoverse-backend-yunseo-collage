namespace Collage.Backend.Induction.Starter.Models
{
    public class HealthResponse
    {
        public string Status { get; set; } = "ok";
        public string Service { get; set; } = "collage-backend-induction-starter";
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;
    }
}
