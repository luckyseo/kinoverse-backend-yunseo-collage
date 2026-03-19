using Models;
using System.Text.Json.Serialization;
//review
// BC: not much to say here, straightforward DTO for tagging emotion
namespace DTOs
{
//     { example request body
//   "emotion": "happy",
//   "userId": "user-123" 
//     }

    public class TagEmotionRequestDto
    {
        public string Emotion { get; set; }
        public string UserId { get; set; }
    }
}