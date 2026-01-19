using Models;
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
    public class MovieDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public int ReleaseYear { get; set; }
        public string PosterUrl { get; set; }
        public List<Genre> Genres { get; set; }
        public int RuntimeMinutes   { get; set; }
        public EmotionStateDto EmotionState {get; set;}
    }
}