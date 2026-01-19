namespace Models.Tmdb
{
    public class TmdbMovieDetailResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public int ReleaseYear { get; set; }
        public string PosterUrl { get; set; }
        public List<int> Genres { get; set; }
        public int RuntimeMinutes   { get; set; }
    }
}