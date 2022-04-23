using MovieCatalog.Enums;
using MovieCatalog.Models;
using MovieCatalog.Models.DTO;

namespace MovieCatalog.Services
{
    public interface IMovieService
    {
        Task<MovieGetShortDTO> PostMovie(MoviePostDTO model);
        List<MovieGetShortDTO> GetAllMovies();
        MovieGetLongDTO GetMovie(int id);
        Task DeleteMovie(int id);
    }

    public class MovieService : IMovieService
    {
        private readonly ApplicationContext _context;

        public MovieService(ApplicationContext context)
        {
            _context = context;
        }

        public List<MovieGetShortDTO> GetAllMovies()
        {
            return _context.Movies.Select(x => new MovieGetShortDTO
            {
                Id = x.Id,
                Name = x.Name,
                Poster = x.Poster,
                Year = x.Year,
                Country = x.Country,
                Genres = x.Genres.Select(y => new GenreDTO { Id = y.Id, Name = y.Name}).ToList()
            }).ToList();
        }

        public async Task<MovieGetShortDTO> PostMovie(MoviePostDTO model)
        {
            var genres = model.Genres.Select(x => GetGenre(x) ?? throw new Exception("Genre does not exist")).ToList();
            var movie = new Movie
            {
                Name = model.Name,
                Year = model.Year,
                Country = model.Country,
                Genres = genres,
                Time = model.Time,
                TagLine = model.TagLine,
                Director = model.Director,
                Budget = model.Budget,
                Fees = model.Fees,
                AgeLimit = model.AgeLimit
            };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return GetShortDTO(movie);
        }

        private MovieGetShortDTO GetShortDTO(Movie movie)
        {
            return new MovieGetShortDTO
            {
                Id = movie.Id,
                Name = movie.Name,
                Poster = movie.Poster,
                Year = movie.Year,
                Country = movie.Country,
                Genres = movie.Genres.Select(x => new GenreDTO { Id = x.Id, Name = x.Name }).ToList()
            };
        }

        private Genre? GetGenre(int id)
        {
            return _context.Genres.FirstOrDefault(x => x.Id == id);
        }

        public MovieGetLongDTO GetMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == null) throw new Exception("Movie does not exist");
            return new MovieGetLongDTO
            {
                Id = movie.Id,
                Name = movie.Name,
                Poster = movie.Poster,
                Year = movie.Year,
                Country = movie.Country,
                Genres = movie.Genres.Select(y => new GenreDTO { Id = y.Id, Name = y.Name }).ToList(),
                Time = movie.Time,
                TagLine = movie.TagLine,
                Director = movie.Director,
                Budget = movie.Budget,
                Fees = movie.Fees,
                AgeLimit = movie.AgeLimit
            };
        }

        public async Task DeleteMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == null) throw new Exception("Movie does not exist");
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}
