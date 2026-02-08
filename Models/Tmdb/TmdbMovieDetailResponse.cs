using System.Text.Json.Serialization;
using Models;
//review
// BC: TMDB Movie Detail Response model Maps properly
// Well defined properties with JsonPropertyName attributes for accurate serialization
// Would maybe split TMDBGenre into its own file for larger projects but fine here
namespace Models.Tmdb
{
    public class TmdbMovieDetailResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }
        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }
        public List<TmdbGenre> Genres { get; set; } = new List<TmdbGenre>();
        public int? Runtime   { get; set; }
    }
    public class TmdbGenre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}