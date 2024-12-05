using ClinicAPI.Models;

namespace ClinicAPI.DTOs
{
    public class DoctorDTOs
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? PersonalId { get; set; }
        public int? Role { get; set; } 
        public string? Category { get; set; }
        public int? Rating { get; set; }
        public IFormFile? Photo { get; set; }
        public IFormFile? Cv { get; set; }

    }
}
