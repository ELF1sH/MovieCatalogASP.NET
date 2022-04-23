using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Models
{
    public class DeactivatedToken
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
