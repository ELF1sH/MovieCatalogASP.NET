using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MovieCatalog.Models.DTO;
using MovieCatalog.Services;

namespace MovieCatalog.Controllers
{
    [ApiController]
    [Route("/api/movies")]
    public class PosterController : Controller
    {
        private readonly IDeactivateTokenService _deactivateTokenService;
        private readonly IPosterService _posterService;

        public PosterController(IDeactivateTokenService deactivateToken, IPosterService posterService)
        {
            _deactivateTokenService = deactivateToken;
            _posterService = posterService;
        }

        [HttpPost]
        [Route("{id:int}/poster")]
        [Authorize(Roles = "Admin")]
        public IActionResult PostPoster([FromRoute] int id, [FromForm] IFormFile file)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (_deactivateTokenService.IsTokenDeactivated(accessToken))
            {
                return StatusCode(401, new { message = "Token has been deactivated" });
            }

            if (Path.GetExtension(file.FileName) != ".jpg" && Path.GetExtension(file.FileName) != ".png")
            {
                return StatusCode(400, new { message = "Wrong file extension. Consider uploading jpg or png file" });
            }

            try
            {
                return new JsonResult(_posterService.PostPoster(id, file).Result);
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
        [Route("{id:int}/poster")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePoster([FromRoute] int id)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (_deactivateTokenService.IsTokenDeactivated(accessToken))
            {
                return StatusCode(401, new { message = "Token has been deactivated" });
            }

            try
            {
                return new JsonResult(_posterService.DeletePoster(id));
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
