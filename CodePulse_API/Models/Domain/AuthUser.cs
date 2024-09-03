using Microsoft.AspNetCore.Identity;

namespace CodePulse_API.Models.Domain
{
    public class AuthUser : IdentityUser
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
