namespace FYPIBDPatientApp.Dtos
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; } 
        public string MobileNumber { get; set; }
    }
}
