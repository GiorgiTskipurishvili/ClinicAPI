using System;

namespace ClinicAPI.Models
{
    public class Doctor
    {
        //public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public int PersonalId { get; set; }
        //public string Category { get; set; }
        //public byte[] Photo { get; set; } // Assuming BLOB maps to byte array
        //public byte[] Cv { get; set; } // Assuming BLOB maps to byte array
        //public int Role { get; set; } // Reference Role enum values
        //public double Rating { get; set; }


        public long Id { get; set; }  // Changed to long
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long PersonalId { get; set; }  // Changed to long
        public string Category { get; set; }
        public int? Role { get; set; }
        public double? Rating { get; set; }
        public byte[]? Photo { get; set; }
        public byte[]? Cv { get; set; }
    }
}
