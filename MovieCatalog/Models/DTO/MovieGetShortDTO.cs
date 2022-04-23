namespace MovieCatalog.Models.DTO
{
    public class MovieGetShortDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Poster { get; set; }

        public int Year { get; set; }

        public string Country { get; set; }

        public List<GenreDTO> Genres { get; set; }
    }
}
