using System.Text.Json.Serialization;
namespace Models
//review
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