using CodePulse_API.Models.Domain;
using CodePulse_API.Models.DTO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CodePulse_API.Repositories.Interface
{
    public interface ITokenRepository
    {
        //public string CreateJwtToken(AuthUser user, List<string> roles);
        public string CreateJwtToken(AuthUser user);
        public string CreateRefreshToken(string refreshToken);
        public DateTime SetRefreshTokenExpiry();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        Task<AuthUser> UpdateJwtToken(AuthUser user);

        Task<TokenApiDto> RefreshToken(TokenApiDto responseDto);
    }
}
