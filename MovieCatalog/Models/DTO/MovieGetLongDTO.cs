namespace MovieCatalog.Models.DTO
{
    public class MovieGetLongDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Poster { get; set; }

        public int Year { get; set; }

        public string Country { get; set; }

        public List<GenreDTO> Genres { get; set; }

        public int Time { get; set; }

        public string? TagLine { get; set; }

        public string? Director { get; set; }

        public int? Budget { get; set; }

        public int? Fees { get; set; }

        public int AgeLimit { get; set; }
    }
}
