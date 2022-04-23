using MovieCatalog.Models;
using MovieCatalog.Models.DTO;

namespace MovieCatalog.Services
{
    public interface IUserService
    {
        UserGetDTO? GetUser(int id);
        int? GetIdByUsername(string username);
        Task EditUser(int id, UserPutDTO model);
    }

    public class UserService : IUserService
    {
        private readonly ApplicationContext _context;

        public UserService(ApplicationContext context)
        {
            _context = context;
        }

        public UserGetDTO? GetUser(int id)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return null;
            return new UserGetDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
            };
        }

        public int? GetIdByUsername(string username)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == username);
            if (user == null) return null;
            return user.Id;
        }

        public async Task EditUser(int id, UserPutDTO model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return;
            user.Username = model.Username;
            user.Email = model.Email;
            user.Name = model.Name;
            user.BirthDate = model.BirthDate;
            user.Gender = model.Gender;
            await _context.SaveChangesAsync();
        }
    }
}
