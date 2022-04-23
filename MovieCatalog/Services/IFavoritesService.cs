using MovieCatalog.Models;
using MovieCatalog.Models.DTO;

namespace MovieCatalog.Services
{
    public interface IFavoritesService
    {
        Task AddFavorites(int userId, int movieId);
        List<MovieGetShortDTO> GetAllFavorites(int userId);
        Task DeleteFavorite(int userId, int movieId);
    }

    public class FavoritesService : IFavoritesService
    {
        private readonly ApplicationContext _context;

        public FavoritesService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddFavorites(int userId, int movieId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null) throw new Exception("User does not exist");

            var movie = _context.Movies.FirstOrDefault(x => x.Id == movieId);
            if (movie == null) throw new Exception("Movie does not exist");

            user.Favorites.Add(movie);
            await _context.SaveChangesAsync();
        }

        public List<MovieGetShortDTO> GetAllFavorites(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null) throw new Exception("User does not exist");

            return user.Favorites.Select(x => new MovieGetShortDTO
            {
                Id = x.Id,
                Name = x.Name,
                Poster = x.Poster,
                Year = x.Year,
                Country = x.Country,
                Genres = x.Genres.Select(y => new GenreDTO { Id = y.Id, Name = y.Name }).ToList()
            }).ToList();
        }

        public async Task DeleteFavorite(int userId, int movieId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null) throw new Exception("User does not exist");

            var movie = _context.Movies.FirstOrDefault(x => x.Id == movieId);
            if (movie == null) throw new Exception("Movie does not exist");

            user.Favorites.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}
