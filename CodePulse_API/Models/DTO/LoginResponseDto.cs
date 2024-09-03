namespace CodePulse_API.Models.DTO
{
    public class LoginResponseDto
    {
        public string eMail { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public List<string> Roles { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }

    }
}
