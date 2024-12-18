﻿using System;

namespace ClinicAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public long UserId { get; set; }
        public long DoctorId { get; set; }
        public string Description { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
