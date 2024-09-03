using CodePulse_API.Data;
using CodePulse_API.Models.Domain;
using CodePulse_API.Models.DTO;
using CodePulse_API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace CodePulse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AuthUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<AuthUser> userManager, ITokenRepository tokenRepository)
        
        {
            this.tokenRepository = tokenRepository;
            this.userManager = userManager;
        }

        // apibasrUrl/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request) {

            var identityUser =  await userManager.FindByEmailAsync(request.Email);           
            if (identityUser is not null) {
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

               if (checkPasswordResult) {
                   
                    var roles = await userManager.GetRolesAsync(identityUser);
                    // Create Access Token
                    var jwttoken = tokenRepository.CreateJwtToken(identityUser);

                    var authUsers = new AuthUser() { 
                        Id = identityUser.Id,
                        UserName = identityUser.UserName,
                        NormalizedUserName = identityUser.NormalizedUserName,
                        Email = identityUser.Email,
                        NormalizedEmail = identityUser.NormalizedEmail,
                        PasswordHash = identityUser.PasswordHash,
                        Token = jwttoken,
                        RefreshToken = identityUser.RefreshToken,
                        RefreshTokenExpiryTime = identityUser.RefreshTokenExpiryTime
                    };

                    if (authUsers.RefreshToken == null 
                        && (authUsers.RefreshTokenExpiryTime.ToString() == "0001-01-01 00:00:00.0000000" 
                        || authUsers.RefreshTokenExpiryTime <= DateTime.Now))
                    {
                        // Create Refresh Token
                        authUsers.RefreshToken = tokenRepository.CreateRefreshToken(authUsers.RefreshToken);
                        authUsers.RefreshTokenExpiryTime = tokenRepository.SetRefreshTokenExpiry();
                    }
                    // Store Access & Refresh token in DB
                    var result = await tokenRepository.UpdateJwtToken(authUsers);

                    var returnResponse = new LoginResponseDto() {
                        AccessToken = authUsers.Token,
                        RefreshToken = authUsers.RefreshToken,
                        RefreshTokenExpiryTime = authUsers.RefreshTokenExpiryTime,
                        eMail = authUsers.Email,
                        Roles = roles.ToList()
                    };

                    return Ok(returnResponse);
                }
            }

            ModelState.AddModelError("", "Email or Passord Incorrect");
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Create Identity User Object
            var user = new AuthUser
            {
                UserName = request.eMail.Trim(),
                Email = request.eMail.Trim()
                
            };
           var identityResult = await userManager.CreateAsync(user, request.password);
            //var role = request.role.Trim();

            if (identityResult.Succeeded)
            {
                // Add Role to user (Reader)
                identityResult = await userManager.AddToRoleAsync(user, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

            }
            else {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);

        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken(TokenApiDto responseDto) {

            if (responseDto is null)
                return BadRequest("Invalid Client Request");

            //var res = tokenRepository.CreateRefreshToken(responseDto.RefreshToken);
            var res = await tokenRepository.RefreshToken(responseDto);

            return Ok(res);
            //return null;
        }
    }
}
