using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Models.DTO
{
    public class UserRegisterDTO
    {
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
    }
}
