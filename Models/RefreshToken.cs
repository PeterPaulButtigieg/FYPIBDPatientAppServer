﻿namespace FYPIBDPatientApp.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public bool isRevoked { get; set; }
    }
}
