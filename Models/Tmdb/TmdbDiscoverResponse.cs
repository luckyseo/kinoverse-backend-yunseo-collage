using System.Collections.Generic;
using System.Text.Json.Serialization;
//review
// BC: TMDB Discover Response model Maps properly
// Well defined properties with JsonPropertyName attributes for accurate serialization
namespace Models.Tmdb
{
    public class TmdbDiscoverResponse
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<TmdbMovieItem> Results { get; set; } = new();
    }
    public class TmdbMovieItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } // Keep as string for safety, parse later

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; } // Corresponds to the raw API field
    }
}