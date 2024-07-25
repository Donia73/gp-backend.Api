using Microsoft.AspNetCore.Identity;

namespace gp_backend.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<RefreshToken>? RefreshTokens { get; set; } = new();
    }
}
