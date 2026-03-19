using System.Text.Json.Serialization;
using Models;
//review
// BC: Tag Emotion Response DTO looks good
// Clear properties and proper use of EmotionStateDto
namespace DTOs
{
    public class TagEmotionResponseDto
    {
        public string MovieId { get; set; }
        public EmotionStateDto EmotionState { get; set; }
    }
}