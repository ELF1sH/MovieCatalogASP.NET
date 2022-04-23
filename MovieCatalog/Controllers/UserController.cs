using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MovieCatalog.Models.DTO;
using MovieCatalog.Services;

namespace MovieCatalog.Controllers
{
    [ApiController]
    [Route("/api/account/profile")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDeactivateTokenService _deactivateTokenService;

        public UserController(IUserService userService, IDeactivateTokenService deactivateToken)
        {
            _userService = userService;
            _deactivateTokenService = deactivateToken;
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize]
        public IActionResult GetUser([FromRoute] int id)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (_deactivateTokenService.IsTokenDeactivated(accessToken))
            {
                return StatusCode(401, new { message = "Token has been deactivated" });
            }

            string usernameClaim = User.Claims.ToList()[0].ToString();
            string username = usernameClaim.Substring(usernameClaim.IndexOf(" ") + 1);

            string roleClaim = User.Claims.ToList()[1].ToString();
            string role = roleClaim.Substring(roleClaim.IndexOf(" ") + 1);

            if (role == "Admin" || _userService.GetIdByUsername(username) == id)
            {
                var user = _userService.GetUser(id);
                if (user == null)
                {
                    return StatusCode(400, new { message = "User with such id does not exist" });
                }
                else
                {
                    return new JsonResult(user);
                }
            }
            else
            {
                return StatusCode(403, new { message = "Only admin or data owner has access to this data" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize]
        public async Task<IActionResult> EditUser([FromRoute] int id, UserPutDTO model)
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (_deactivateTokenService.IsTokenDeactivated(accessToken))
            {
                return StatusCode(401, new { message = "Token has been deactivated" });
            }

            string usernameClaim = User.Claims.ToList()[0].ToString();
            string username = usernameClaim.Substring(usernameClaim.IndexOf(" ") + 1);

            string roleClaim = User.Claims.ToList()[1].ToString();
            string role = roleClaim.Substring(roleClaim.IndexOf(" ") + 1);

            if (role == "Admin" || _userService.GetIdByUsername(username) == id)
            {
                var user = _userService.GetUser(id);
                if (user == null)
                {
                    return StatusCode(400, new { message = "User with such id does not exist" });
                }
                else
                {
                    try
                    {
                        await _userService.EditUser(id, model);
                        return Ok(new {message = "OK"});
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
            else
            {
                return StatusCode(403, new { message = "Only admin or data owner has access to this method" });
            }
        }
    }
}
