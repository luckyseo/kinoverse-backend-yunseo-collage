using Models;
using System.Text.Json.Serialization;

namespace DTOs
{
//     { example request body
//   "emotion": "happy",
//   "userId": "user-123" 
//     }

    public class TagEmotionRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType? Emotion { get; set; }
        public string UserId { get; set; }
    }
}