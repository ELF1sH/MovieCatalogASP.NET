using MovieCatalog.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Models
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Poster { get; set; } = null;

        [Required]
        public int Year { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int Time { get; set; }

        public string? TagLine { get; set; } = null;

        public string? Director { get; set; } = null;

        public int? Budget { get; set; } = null;

        public int? Fees { get; set; } = null;

        [Required]
        public int AgeLimit { get; set; }

        [Required]
        public List<Genre> Genres { get; set; } = new List<Genre>();

        [Required]
        public List<User> Users { get; set; } = new List<User>();
    }
}
