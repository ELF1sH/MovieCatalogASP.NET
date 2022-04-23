namespace MovieCatalog.Models.DTO
{
    public class UserGetDTO
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public int Gender { get; set; }
    }
}
