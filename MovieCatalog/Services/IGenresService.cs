using MovieCatalog.Models;
using MovieCatalog.Models.DTO;

namespace MovieCatalog.Services
{
    public interface IGenresService
    {
        List<GenreDTO> GetAllGenres();
    }

    public class GenresService : IGenresService
    {
        private readonly ApplicationContext _context;

        public GenresService(ApplicationContext context)
        {
            _context = context;
        }

        public List<GenreDTO> GetAllGenres()
        {
            return _context.Genres.Select(x => new GenreDTO
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
        }
    }
}
