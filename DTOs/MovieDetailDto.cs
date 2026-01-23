using Models;
using System.Text.Json.Serialization;

namespace DTOs
{
    /*
    id
    title
    releaseYear (derived from release_date)
    posterUrl (full URL, not just TMDb path)
    overview (short description; you may truncate)
    emotionSummary (your app’s custom emotion metadata)

    Emotion metadata is custom to KinoVerse, not TMDb.
        So:
        If the movie has never been tagged in KinoVerse → return counts as 0 (or an empty summary)
        If it has been tagged → return the real emotion counts + user emotion (if known)
        This keeps the API contract stable and avoids “sometimes missing fields”.    
    */
    public class MovieDetailDto : Movie
    {
        public MovieDetailDto() {}

        public MovieDetailDto(int id, string title, string overview, string releaseYear, string posterUrl, List<int> genres, int runtimeMinutes, EmotionStateDto emotionState)
            : base(id, title, overview, releaseYear, posterUrl)
        {
            Genres = genres;
            RuntimeMinutes = runtimeMinutes;
            EmotionState = emotionState;
        }
        [JsonPropertyOrder(8)]
        public List<int> Genres { get; set; }//filter it using genre dictionary on the service layer
        [JsonPropertyOrder(9)]
        public int RuntimeMinutes   { get; set; }
        [JsonPropertyOrder(10)]
        public EmotionStateDto EmotionState {get; set;}
    }
}