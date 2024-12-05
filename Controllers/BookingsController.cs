using ClinicAPI.DTOs;
using ClinicAPI.Models;
using ClinicAPI.Packages;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : Controller
    {
        [HttpPost]
        public IActionResult AddBooking([FromBody] BookingDTOs bookingDTOs)
        {
            if (bookingDTOs == null)
            {
                return BadRequest("Booking request cannot be null.");
            }

            try
            {
                PKG_BOOKINGS pkgBookings = new PKG_BOOKINGS();
                pkgBookings.add_booking(bookingDTOs);

                return Ok("Booking added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet]
        public IActionResult GetBookings()
        {
            try
            {
                PKG_BOOKINGS pkgBookings = new PKG_BOOKINGS();
                List<Booking> bookings = pkgBookings.get_bookings();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        //[HttpDelete("{id}")]
        //public IActionResult DeleteBooking(int id)
        //{
        //    try
        //    {
        //        PKG_BOOKINGS pkgBookings = new PKG_BOOKINGS();
        //        pkgBookings.delete_booking(id);

        //        return Ok($"Booking with ID {id} deleted successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int id)
        {
            try
            {
                PKG_BOOKINGS pkgBookings = new PKG_BOOKINGS();
                pkgBookings.delete_booking(id);

                // Return a JSON response instead of plain text
                return Ok(new { message = $"Booking with ID {id} deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }




        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int id, [FromBody] Booking booking)
        {
            if (booking == null || booking.Id != id)
            {
                return BadRequest("Invalid booking data.");
            }

            try
            {
                PKG_BOOKINGS pkgBookings = new PKG_BOOKINGS();
                pkgBookings.update_booking(booking);

                return Ok($"Booking with ID {id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("{id}")]
        public IActionResult GetBookingById(int id)
        {
            try
            {
                PKG_BOOKINGS pkgBookings = new PKG_BOOKINGS();
                Booking booking = pkgBookings.get_booking_by_id(id);

                if (booking == null)
                {
                    return NotFound($"Booking with ID {id} not found.");
                }

                return Ok(booking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        //[HttpGet("{userId}")]
        //public IActionResult GetBookingByUserId(int userId)
        //{
        //    try
        //    {
        //        PKG_BOOKINGS pKG_BOOKINGS = new PKG_BOOKINGS();
        //        Booking booking = pKG_BOOKINGS.get_booking_by_userId(userId);
        //        if(booking == null)
        //        {
        //            return NotFound($"Booking with ID {userId} not found.");
        //        }
        //        return Ok(booking);
        //    }
        //    catch(Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        [HttpGet("user/{userId}")]
        public IActionResult GetByUserId(long userId)
        {
            try
            {
                PKG_BOOKINGS pkgBookings = new PKG_BOOKINGS();
                var bookings = pkgBookings.get_by_userId(userId);

                if (bookings.Count == 0)
                {
                    return NotFound($"No bookings found for user ID {userId}.");
                }

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("doctor/{doctorId}")]
        public IActionResult GetByDoctorId(long doctorId)
        {
            try
            {
                PKG_BOOKINGS pkgBookings = new PKG_BOOKINGS();
                var bookings = pkgBookings.get_by_doctorId(doctorId);

                if (bookings.Count == 0)
                {
                    return NotFound($"No bookings found for doctor ID {doctorId}.");
                }

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




    }
}
