
using DTOs;
//review
// BC: you can probably use reflection to get all EmotionType values instead of hardcoding them for initialization
// But this is fine for now, just something to consider for scalability
namespace Models
{
    public class MovieEmotionState
    {
        //This part is already defined on KinoVerse BE description

        // & “How many users selected each emotion for this movie?”
        // {
        // "happy": 3,
        // "mindBlown": 5,
        // "cosy": 1,
        // "scary": 0
        // }
         // for this movie, what user selected what emotion
        // {
        // "user-123": "happy",
        // "user-456": "mindBlown",
        // "user-789": null
        // }
        public Dictionary<EmotionType, int> EmotionCounts { get; set; } //shared across all users
        public Dictionary<string, EmotionType?> UserEmotionsByUserId { get; set; } //per user
        
        public MovieEmotionState()
        {
            EmotionCounts = new Dictionary<EmotionType, int>
            {
                { EmotionType.Happy, 0 },
                { EmotionType.MindBlown, 0 },
                { EmotionType.Cosy, 0 },
                { EmotionType.Scary, 0 },
                {EmotionType.Motivational,0 },
                {EmotionType.Sad,0 }
            };
            UserEmotionsByUserId = new Dictionary<string, EmotionType?>();
        }

    }
}