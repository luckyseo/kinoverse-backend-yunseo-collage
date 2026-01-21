using System.Text.Json.Serialization;
using Models;
namespace DTOs
{
    public class MovieSummaryDto
    {
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public int ReleaseYear { get; set; }
        public string PosterUrl { get; set; }
        public EmotionSummary EmotionSummary { get; set; }
    }

    public class EmotionSummary
    {
        public List<TopEmotion> TopEmotions { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType? UserEmotion { get; set; }
    }

    public class TopEmotion
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType? Emotion { get; set; }
        public int Count { get; set; }
    }
}