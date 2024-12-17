using System;

namespace ClinicAPI.Models
{
    public class Doctor
    {

        public long Id { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long PersonalId { get; set; }  
        public string Category { get; set; }
        public int? Role { get; set; }
        public double? Rating { get; set; }
        public byte[]? Photo { get; set; }
        public byte[]? Cv { get; set; }
    }
}
