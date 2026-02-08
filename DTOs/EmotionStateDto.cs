using Models;
using System.Text.Json.Serialization;
namespace DTOs
//review
// BC: Good Used of Enum Converter for UserEmotion
{
    public class EmotionStateDto
    {
        public Dictionary<EmotionType, int> Counts { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType? UserEmotion { get; set; }
    }

}