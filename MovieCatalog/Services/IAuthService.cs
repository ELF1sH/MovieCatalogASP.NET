using Microsoft.IdentityModel.Tokens;
using MovieCatalog.Enums;
using MovieCatalog.Models;
using MovieCatalog.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MovieCatalog.Services
{
    public interface IAuthService
    {
        Task RegisterUser(UserRegisterDTO model);
        string GetToken(string username, Role role);
        User? DoesUserExist(UserLoginDTO model);
    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationContext _context;

        public AuthService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task RegisterUser(UserRegisterDTO model)
        {
            await _context.Users.AddAsync(new User
            {
                Username = model.Username,
                Name = model.Name,
                Password = EncodePassword(model.Password),
                Email = model.Email,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Role = 0
            });
            await _context.SaveChangesAsync();
        }

        public string GetToken(string username, Role role)
        {
            ClaimsIdentity identity = GetIdentity(username, role);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtConfigurations.Lifetime)),
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private static ClaimsIdentity GetIdentity(string username, Role role)
        {
            // Claims описывают набор базовых данных для авторизованного пользователя
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
            };

            //Claims identity и будет являться полезной нагрузкой в JWT токене, которая будет проверяться стандартным атрибутом Authorize
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }

        private static string EncodePassword(string password)
        {
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            var sb = new System.Text.StringBuilder();
            foreach (byte b in hash)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        public User? DoesUserExist(UserLoginDTO model)
        {
            model.Password = EncodePassword(model.Password);
            return _context.Users.FirstOrDefault(x => x.Username == model.Username && x.Password == model.Password);
        }
    }
}
