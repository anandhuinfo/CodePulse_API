namespace CodePulse_API.Models.DTO
{
    public class LoginResponseDto
    {
        public string eMail { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
