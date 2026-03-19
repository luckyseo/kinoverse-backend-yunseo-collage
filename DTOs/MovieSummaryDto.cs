using System.Text.Json.Serialization;
using Models;
namespace DTOs
//review
// BC: Consider Splitting the EmotionSummary and TopEmotion classes into separate files for better organization
// Imagine this, what would happen if we have more emotion-related DTOs in the future?
// Or even different uses like, say, a genre emotion summary?
// Just a thought for scalability and maintainability
// Based on current scope, it functions as intended
//
// Other than that, the MovieSummaryDto looks well structured and clear
{
    public class MovieSummaryDto : Movie
    {
        private string _overview;
        [JsonPropertyOrder(9)]
        public string Overview { 
            get { return _overview; }
            set {   
                if (value.Length > 50)
                {
                    _overview = value.Substring(0, 47) + "...";
                }
                else
                {
                    _overview = value;
                }
            }
        }
        [JsonPropertyOrder(10)]
        public EmotionSummary EmotionSummary { get; set; }

        public MovieSummaryDto() {}
        public MovieSummaryDto(int id, string title, string overview, string releaseYear, string posterUrl, EmotionSummary emotionSummary)
            : base(id, title, releaseYear, posterUrl)
        {
            Overview = overview;
            EmotionSummary = emotionSummary;
        }
    }

    public class EmotionSummary
    {
        public List<TopEmotion> TopEmotions { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType? UserEmotion { get; set; }

        public EmotionSummary()
        {
            TopEmotions = new List<TopEmotion>();
            UserEmotion = null;
        }
        public EmotionSummary(List<TopEmotion> topEmotions, EmotionType? userEmotion)
        {
            TopEmotions = topEmotions;
            UserEmotion = userEmotion;
        }
    }

    public class TopEmotion
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmotionType? Emotion { get; set; }
        public int Count { get; set; }

        public TopEmotion() {}
        public TopEmotion(EmotionType? emotion, int count)
        {
            Emotion = emotion;
            Count = count;
        }
    }
}