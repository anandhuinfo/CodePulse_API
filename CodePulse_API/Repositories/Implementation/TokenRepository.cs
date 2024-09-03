using CodePulse_API.Data;
using CodePulse_API.Models.Domain;
using CodePulse_API.Models.DTO;
using CodePulse_API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CodePulse_API.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;
        private readonly AuthDbContext dbAuthContext;

        public TokenRepository(IConfiguration configuration, AuthDbContext authDbContext)
        {
            this.configuration = configuration;
            this.dbAuthContext = authDbContext;
        }

       public string CreateJwtToken(AuthUser user) {
            // Create Claims
            var claims = new List<Claim> {
            new Claim(ClaimTypes.Email, user.Email),
            };

            List<string> roles = new List<string>();
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // JWT Security Token

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer : configuration["Jwt: Issuer"],
                audience: configuration["Jwt: Audience"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(Convert.ToInt64(configuration["Jwt:TokenExpiryTimeinseconds"])),
                signingCredentials: credentials
                );
            // Return Token
            return new JwtSecurityTokenHandler().WriteToken(token); 
        
        }

       public string CreateRefreshToken(string? token)
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);
            var tokeninUser = token;
            return refreshToken;
        }

        public DateTime SetRefreshTokenExpiry() {
            
                return DateTime.Now.AddDays(Convert.ToInt64(configuration["Jwt:RefreshTokenExpiryTimeinDays"]));
        } 

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token) {
            try
            {
                var jwtConfiguration = configuration["jwt"];
                var tokenValidationParameter = new TokenValidationParameters
                {
                    AuthenticationType = "Jwt",
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token.Replace("Bearer ", string.Empty), tokenValidationParameter, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;

                if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invaild Token");
                }
                return principal;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        // Save Token in DB
        public async Task<AuthUser> UpdateJwtToken(AuthUser user) {
            var existingAuthUser = await dbAuthContext.AuthUsers.FirstOrDefaultAsync(x => x.Id == user.Id);

            if (existingAuthUser != null)
            {
                dbAuthContext.Entry(existingAuthUser).CurrentValues.SetValues(user);
                 await dbAuthContext.SaveChangesAsync();
                return user;
            }
            return null;
        }


        // Regenerate new Token After expiry
        public async Task<TokenApiDto> RefreshToken(TokenApiDto responseDto)
        {
            if (responseDto is null)
                return null;

            string accessToken = responseDto.AccessToken;
            string refreshToken = responseDto.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity.Name;
            var user = await dbAuthContext.AuthUsers.FirstOrDefaultAsync(x => x.UserName == userName);
            var roles = await dbAuthContext.Roles.FirstOrDefaultAsync(x => x.Name == userName);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime < DateTime.Now)
                return null;

            var newAccessToken = CreateJwtToken(user);
            var newRefreshToken = CreateRefreshToken(responseDto.RefreshToken);

            user.RefreshToken = newRefreshToken;
            await dbAuthContext.SaveChangesAsync();

            var res = new TokenApiDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return res;
        }


    }
}
