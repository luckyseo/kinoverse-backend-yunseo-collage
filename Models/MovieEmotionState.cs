
namespace Models
{
    public class MovieEmotionState
    {
        public Dictionary<EmotionType, int> EmotionCounts { get; set; }
        public Dictionary<string, EmotionType?> UserEmotionsByUserId { get; set; }
    }
}