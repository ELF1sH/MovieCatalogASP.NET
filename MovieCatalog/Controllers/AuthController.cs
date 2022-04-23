using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MovieCatalog.Enums;
using MovieCatalog.Models.DTO;
using MovieCatalog.Services;

namespace MovieCatalog.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IDeactivateTokenService _deactivateTokenService;

        public AuthController(IAuthService auth, IDeactivateTokenService deactivateToken)
        {
            _authService = auth;
            _deactivateTokenService = deactivateToken;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO model)
        {
            try
            {
                var gender = (Gender)Enum.ToObject(typeof(Gender), model.Gender);
                if (int.TryParse(gender.ToString(), out _))
                {
                    return StatusCode(500, new { message = "Invalid gender code" });
                }

                await _authService.RegisterUser(model);
                return new JsonResult(new { token = _authService.GetToken(model.Username, Role.SimpleUser) });
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

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UserLoginDTO model)
        {
            try
            {
                var user = _authService.DoesUserExist(model);
                if (user == null)
                {
                    return StatusCode(500, new { message = "Invalid login or password" });
                }
                return new JsonResult(new { token = _authService.GetToken(user.Username, (Role)Enum.ToObject(typeof(Role), user.Role)) });
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

        [HttpPost]
        [Authorize]
        [Route("logout")]
        public IActionResult Post()
        {
            var accessToken = Request.Headers[HeaderNames.Authorization];
            if (_deactivateTokenService.IsTokenDeactivated(accessToken))
            {
                return StatusCode(401, new { message = "Token has been deactivated" });
            }

            _deactivateTokenService.DeactivateToken(accessToken);
            return new JsonResult(new
            {
                message = "OK"
            });
        }
    }
}
