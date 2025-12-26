using Application.Common.Interfaces.Identity;

namespace Infrastructure.Common.Interfaces.Identity
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(userIdStr, out var userId) ? userId : Guid.Empty;
            }
        }

        public IEnumerable<string> Roles
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?
                    .FindAll(ClaimTypes.Role)
                    .Select(r => r.Value)
                    ?? Enumerable.Empty<string>();
            }
        }
    }

}
