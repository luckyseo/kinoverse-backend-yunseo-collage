using Models;
using System.Text.Json.Serialization;
//review
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