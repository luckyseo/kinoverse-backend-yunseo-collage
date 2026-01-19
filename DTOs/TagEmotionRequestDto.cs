using Models;
using System.Text.Json.Serialization;

namespace DTOs
{
    public class TagEmotionRequestDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType Emotion { get; set; }
        public string UserId { get; set; }
    }
}