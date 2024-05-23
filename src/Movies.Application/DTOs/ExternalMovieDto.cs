namespace Movies.Application.DTOs
{
    public class ExternalMovieDto
    {
        public ExternalMovieDto(string id, string rank, string title, string fullTitle, string year, string image, string crew, string imdbRating, string imdbRatingCount)
        {
            Id = id;
            Rank = rank;
            Title = title;
            FullTitle = fullTitle;
            Year = year;
            Image = image;
            Crew = crew;
            ImdbRating = imdbRating;
            ImdbRatingCount = imdbRatingCount;
        }

        public ExternalMovieDto()
        { }

        public string Id { get; set; }
        public string Rank { get; set; }
        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string Year { get; set; }
        public string Image { get; set; }
        public string Crew { get; set; }
        public string ImdbRating { get; set; }
        public string ImdbRatingCount { get; set; }
    }
}
