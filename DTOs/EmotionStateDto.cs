using Models;
using System.Text.Json.Serialization;
namespace DTOs
//review
{
    public class EmotionStateDto
    {
        public Dictionary<EmotionType, int> Counts { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType? UserEmotion { get; set; }
    }

}