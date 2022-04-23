namespace MovieCatalog.Models.DTO
{
    public class MoviePostDTO
    {
        public string Name { get; set; }

        public int Year { get; set; }

        public string Country { get; set; }

        public List<int> Genres { get; set; }

        public int Time { get; set; }

        public string? TagLine { get; set; }

        public string? Director { get; set; }

        public int? Budget { get; set; }

        public int? Fees { get; set; }

        public int AgeLimit { get; set; }
    }
}
