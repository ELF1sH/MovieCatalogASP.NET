using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Services;

namespace MovieCatalog.Controllers
{
    [ApiController]
    [Route("/api/genres")]
    public class GenresController : Controller
    {
        private readonly IGenresService _genresService;

        public GenresController(IGenresService genresService)
        {
            _genresService = genresService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAllGenres()
        {
            try
            {
                return new JsonResult(_genresService.GetAllGenres());
            }
            catch (Exception ex)
            {
                var response = new
                {
                    message = ex.InnerException == null ? ex.Message : ex.InnerException.Message
                };
                return StatusCode(500, response);
            }
        }
    }
}
