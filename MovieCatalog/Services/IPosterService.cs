using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Models;
using MovieCatalog.Models.DTO;

namespace MovieCatalog.Services
{
    public interface IPosterService
    {
        Task<MovieGetShortDTO> PostPoster(int id, IFormFile file);
        MovieGetShortDTO DeletePoster(int id);
    }

    public class PosterService : IPosterService
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public PosterService(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<MovieGetShortDTO> PostPoster(int id, IFormFile file)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == null) throw new Exception("movie does not exist");

            string path = GeneratePath(id, Path.GetExtension(file.FileName));
            using var fileStream = new FileStream(_appEnvironment.ContentRootPath + path, FileMode.Create);
            await file.CopyToAsync(fileStream);

            movie.Poster = path;
            await _context.SaveChangesAsync();
            return new MovieGetShortDTO
            {
                Id = id,
                Name = movie.Name,
                Poster = movie.Poster,
                Year = movie.Year,
                Country = movie.Country,
                Genres = movie.Genres.Select(x => new GenreDTO { Id = x.Id, Name = x.Name }).ToList()
            };
        }

        public MovieGetShortDTO DeletePoster(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if (movie == null) throw new Exception("movie does not exist");
            movie.Poster = "";
            _context.SaveChanges();
            return new MovieGetShortDTO
            {
                Id = id,
                Name = movie.Name,
                Poster = movie.Poster,
                Year = movie.Year,
                Country = movie.Country,
                Genres = movie.Genres.Select(x => new GenreDTO { Id = x.Id, Name = x.Name }).ToList()
            };
        }

        private static string GeneratePath(int id, string extension)
        {
            string timeStamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            return $"Files\\{timeStamp}_{new Random().Next(0, 100000)}_poster_of_movie_{id}{extension}";
        }
    }
}
