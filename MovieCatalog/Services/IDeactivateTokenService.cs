using MovieCatalog.Models;

namespace MovieCatalog.Services
{
    public interface IDeactivateTokenService
    {
        Task DeactivateToken(string value);
        bool IsTokenDeactivated(string value);
    }

    public class DeactivateTokenService : IDeactivateTokenService
    {
        private readonly ApplicationContext _context;

        public DeactivateTokenService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task DeactivateToken(string value)
        {
            await _context.DeactivatedTokens.AddAsync(new DeactivatedToken
            {
                Value = value
            });
            _context.SaveChanges();
        }

        public bool IsTokenDeactivated(string value)
        {
            var token = _context.DeactivatedTokens.Where(x => x.Value == value).FirstOrDefault();
            if (token == null)
            {
                return false;
            }
            return true;
        }
    }
}
