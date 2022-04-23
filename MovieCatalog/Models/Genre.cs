using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Models
{
    public class Genre
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
