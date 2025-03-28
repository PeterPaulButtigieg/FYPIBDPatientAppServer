namespace FYPIBDPatientApp.Dtos
{
    public class TokenResultDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
