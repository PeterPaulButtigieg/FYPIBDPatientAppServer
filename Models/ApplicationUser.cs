﻿using Microsoft.AspNetCore.Identity;

namespace FYPIBDPatientApp.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public bool Concent {  get; set; } = false;
        public string Role { get; set; }
    }
}
