using System.Runtime.Serialization;
//review
// BC: This is also food for thought, but have you considered caching the genres based on your searches as well?
// This could help build up the genre dictionary for subsequent queries without needing to hardcode them all here

namespace Models
{
    public static class Genre
    {
        public static readonly IReadOnlyDictionary<int, string> Genres = new Dictionary<int, string>
            {
                {28,"Action"},
                {12,"Abenteuer"},
                {16,"Animation"},
                {35,"Komödie"},
                {80,"Krimi"},
                {99,"Dokumentarfilm"},
                {18,"Drama"},
                {10751,"Familie"},
                {14,"Fantasy"},
                {36,"Historie"},
                {27,"Horror"},
                {10402,"Musik"},
                {9648,"Mystery"},
                {10749,"Liebesfilm"},
                {878,"Science Fiction"},
                {10770,"TV-Film"},
                {53,"Thriller"},
                {10752,"Kriegsfilm"},
                {37,"Western"}
            };

        public static string GetGenreName(int genreId)
        {
            return Genres.ContainsKey(genreId) ? Genres[genreId] : "Unknown";
        }

        public static IEnumerable<string> GetGenreNames(IEnumerable<int> genreIds) //For MovieDetailDto. It returns genrenames
        {
            foreach (var id in genreIds)
            {
                yield return GetGenreName(id);
            }
        }
        public static int GetGenreId(string genreName) //When user type genre name, convert to genre ID
        {
            foreach (var genre in Genres)
            {
                if (genre.Value.Equals(genreName, StringComparison.OrdinalIgnoreCase))
                {
                    return genre.Key;
                }
            }
            return -1; // Return -1 if genre not found
        }
    }
}