using ClinicAPI.DTOs;
using ClinicAPI.Models;
using ClinicAPI.Packages;
using ClinicAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly PKG_DOCTORS _pkgDoctors;

        public DoctorsController(PKG_DOCTORS pkgDoctors)
        {
            _pkgDoctors = pkgDoctors;
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(DoctorDTOs doctorDtos)
        {
            if (doctorDtos.Role != 1)
            {
                return BadRequest($"Error Role: This is not {doctorDtos.Role} Role! The role must be set to {Role.Doctor} (1).");
            }

            if (doctorDtos.Rating < 0 || doctorDtos.Rating > 5)
            {
                return BadRequest("Error: Rating must be between 0 and 5.");
            }

            try
            {
                // Log received data for debugging
                Console.WriteLine($"Received DoctorDTO: {doctorDtos}");

                _pkgDoctors.add_doctor(doctorDtos);
                return Ok(new { message = "Doctor added successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet]
        public IActionResult GetDoctors()
        {
            try
            {
                var doctors = _pkgDoctors.get_doctors();
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            try
            {
                _pkgDoctors.delete_doctor(new Doctor { Id = id });
                return Ok($"Doctor with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(long id, [FromBody] Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return BadRequest("Doctor ID in the URL does not match ID in the body.");
            }

            try
            {
                _pkgDoctors.update_doctor(doctor);
                return Ok($"Doctor with ID {id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetDoctorById(long id)
        {
            try
            {
                var doctor = _pkgDoctors.get_doctor_by_id(new Doctor { Id = id });
                if (doctor == null)
                {
                    return NotFound($"Doctor with ID {id} not found.");
                }

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("email/{email}")]
        public IActionResult GetDoctorByEmail(string email)
        {
            try
            {
                var doctor = _pkgDoctors.get_doctor_by_email(email);
                if (doctor == null)
                {
                    return NotFound($"Doctor with email {email} not found.");
                }

                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        [HttpPost("{id}/extractCvText")]
        public async Task<IActionResult> ExtractCvText(int id)
        {
            try
            {
                var extractedText = await _pkgDoctors.ExtractAndStoreCvTextAsync(id);
                return Ok(new { message = "CV text extracted and stored successfully.", extractedText });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while extracting CV text.", error = ex.Message });
            }
        }
    }
}
