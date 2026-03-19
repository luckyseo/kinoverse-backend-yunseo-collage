using System.Text.Json.Serialization;
namespace Models
//review
// BC: Simple Enum representing different Emotion Types
{
    [JsonConverter(typeof(JsonStringEnumConverter))] //shows string on JSON
    public enum EmotionType
    {
        Happy,
        Sad,
        MindBlown,
        Cosy,
        Scary,
        Motivational
    }
}