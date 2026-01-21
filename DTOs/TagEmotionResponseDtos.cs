using System.Text.Json.Serialization;
using Models;

namespace DTOs
{
    public class TagEmotionResponseDto
    {
        public string MovieId { get; set; }
        public EmotionStateDto EmotionState { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType? UserEmotion { get; set; }
    }
}