using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        public int Role { get; set; }

        [Required]
        public List<Movie> Favorites { get; set; } = new List<Movie>();
    }
}
