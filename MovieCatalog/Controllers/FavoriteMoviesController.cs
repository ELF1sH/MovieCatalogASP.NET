using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MovieCatalog.Models.DTO;
using MovieCatalog.Services;

namespace MovieCatalog.Controllers
{
    [ApiController]
    [Route("/api/favorites")]
    public class FavoriteMoviesController : Controller
    {
        private readonly IFavoritesService _favoritesService;
        private readonly IDeactivateTokenService _deactivateTokenService;
        private readonly IUserService _userService;

        public FavoriteMoviesController(IFavoritesService favoritesService, IDeactivateTokenService deactivateToken, IUserService userService)
        {
            _favoritesService = favoritesService;
            _deactivateTokenService = deactivateToken;
            _userService = userService;
        }

        [HttpPost]
        [Route("{movieId:int}")]
        [Authorize]
        public async Task<IActionResult> AddFavorites([FromRoute] int movieId)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (_deactivateTokenService.IsTokenDeactivated(accessToken))
            {
                return StatusCode(401, new { message = "Token has been deactivated" });
            }

            try
            {
                string usernameClaim = User.Claims.ToList()[0].ToString();
                string username = usernameClaim.Substring(usernameClaim.IndexOf(" ") + 1);
                int? id = _userService.GetIdByUsername(username);
                if (id == null) throw new Exception("User does not exist");

                await _favoritesService.AddFavorites((int)id, movieId);
                return Ok(new { message = "Ok" });
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

        [HttpGet]
        [Route("")]
        [Authorize]
        public IActionResult GetFavorites()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (_deactivateTokenService.IsTokenDeactivated(accessToken))
            {
                return StatusCode(401, new { message = "Token has been deactivated" });
            }

            try
            {
                string usernameClaim = User.Claims.ToList()[0].ToString();
                string username = usernameClaim.Substring(usernameClaim.IndexOf(" ") + 1);
                int? id = _userService.GetIdByUsername(username);
                if (id == null) throw new Exception("User does not exist");

                var movies = _favoritesService.GetAllFavorites((int)id);
                return new JsonResult(movies);
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

        [HttpDelete]
        [Route("{movieId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteFavorite([FromRoute] int movieId)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (_deactivateTokenService.IsTokenDeactivated(accessToken))
            {
                return StatusCode(401, new { message = "Token has been deactivated" });
            }

            try
            {
                string usernameClaim = User.Claims.ToList()[0].ToString();
                string username = usernameClaim.Substring(usernameClaim.IndexOf(" ") + 1);
                int? id = _userService.GetIdByUsername(username);
                if (id == null) throw new Exception("User does not exist");

                await _favoritesService.DeleteFavorite((int)id, movieId);
                return Ok(new { message = "Ok" });
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
