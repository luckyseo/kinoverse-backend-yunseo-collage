namespace Models
//review
// BC: Movie model looks good, with proper handling of ReleaseYear parsing
{
    public class Movie{
        
        private string _releaseYear;
        public int Id { get; set; }
        public string Title { get; set; }
        public string ReleaseYear 
        { 
            get { return _releaseYear; }
            set {
                if (DateTime.TryParse(value, out var date)) //parse to filter only year
                {
                    _releaseYear = date.Year.ToString();
                }
                else
                {
                    _releaseYear = value; //if it's not parseable, keep original
                }
             }
        }
        //tmdbRaw.ReleaseYear != null && DateTime.TryParse(tmdbRaw.ReleaseYear, out var date) ? date.Year : 0,

        public string PosterPath { get; set; }
        public string Overview { get; set; }

        public Movie(int Id, string Title,string Overview, string ReleaseYear, string PosterPath)
        {
            this.Id = Id;
            this.Title = Title;
            this.ReleaseYear = ReleaseYear;
            this.PosterPath = PosterPath;
            this.Overview = Overview;
        }
        public Movie(int Id, string Title, string ReleaseYear, string PosterPath)
        {
            this.Id = Id;
            this.Title = Title;
            this.ReleaseYear = ReleaseYear;
            this.PosterPath = PosterPath;
        }
        public Movie(){}
    }
}