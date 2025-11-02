using Application.Common.Interfaces.Shared;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Common.Interfaces.Shared
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserContext(IHttpContextAccessor httpContextAccessor)
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
