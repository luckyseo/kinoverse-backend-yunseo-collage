
namespace Models
{
    public class MovieEmotionState
    {
        //This part is already defined on KinoVerse BE description

        // & “How many users selected each emotion for this movie?”
        public Dictionary<EmotionType, int> EmotionCounts { get; set; } //shared across all users
        // for this movie, what user selected what emotion
        public Dictionary<string, EmotionType?> UserEmotionsByUserId { get; set; } //per user
    }
}