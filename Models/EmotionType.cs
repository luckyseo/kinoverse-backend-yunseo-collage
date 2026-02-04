using System.Text.Json.Serialization;
namespace Models
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