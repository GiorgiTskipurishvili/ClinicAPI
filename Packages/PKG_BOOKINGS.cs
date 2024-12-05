using ClinicAPI.DTOs;
using ClinicAPI.Models;
using ClinicAPI.Package;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClinicAPI.Packages
{
    public class PKG_BOOKINGS : PKG_BASE
    {

        //public void add_booking(Booking booking)
        //{
        //    OracleConnection conn = new OracleConnection();
        //    conn.ConnectionString = ConnStr;
        //    conn.Open();

        //    OracleCommand cmd = new OracleCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "olerning.PKG_GIORGITSK_BOOKINGS.add_booking";
        //    cmd.CommandType = System.Data.CommandType.Text;

        //    cmd.Parameters.Add("p_user_id", OracleDbType.Int64).Value = booking.UserId;
        //    cmd.Parameters.Add("p_doctor_id", OracleDbType.Int64).Value = booking.DoctorId;
        //    cmd.Parameters.Add("p_description", OracleDbType.Varchar2).Value = booking.Description;
        //    cmd.Parameters.Add("p_booking_time", OracleDbType.Date).Value = booking.BookingTime;

        //    cmd.ExecuteNonQuery();
        //    conn.Close();

        //}

        public void add_booking(BookingDTOs bookingDTOs)
        {
            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_GIORGITSK_BOOKINGS.add_booking", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_user_id", OracleDbType.Int64).Value = bookingDTOs.UserId;
                    cmd.Parameters.Add("p_doctor_id", OracleDbType.Int64).Value = bookingDTOs.DoctorId;
                    cmd.Parameters.Add("p_description", OracleDbType.Varchar2).Value = bookingDTOs.Description;
                    cmd.Parameters.Add("p_booking_time", OracleDbType.Date).Value = bookingDTOs.CreateBookingTime;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (OracleException ex)
                    {
                        // Handle exception, log it or rethrow
                        throw new Exception("Error adding booking", ex);
                    }
                }
            }
        }



        public List<Booking> get_bookings()
        {
            List<Booking> bookings = new List<Booking>();

            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_GIORGITSK_BOOKINGS.get_bookings", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Booking booking = new Booking
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                                Description = reader["description"].ToString(),
                                BookingTime = reader.GetDateTime(reader.GetOrdinal("booking_time")),

                            };

                            bookings.Add(booking);
                        }
                    }
                }
            }

            return bookings;
        }




        public void delete_booking(int bookingId)
        {
            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_GIORGITSK_BOOKINGS.delete_booking", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_id", OracleDbType.Int64).Value = bookingId;

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void update_booking(Booking booking)
        {
            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "olerning.PKG_GIORGITSK_BOOKINGS.update_booking";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("p_id", OracleDbType.Int64).Value = booking.Id;
                    cmd.Parameters.Add("p_user_id", OracleDbType.Int64).Value = booking.UserId;
                    cmd.Parameters.Add("p_doctor_id", OracleDbType.Int64).Value = booking.DoctorId;
                    cmd.Parameters.Add("p_description", OracleDbType.Varchar2).Value = booking.Description;
                    cmd.Parameters.Add("p_booking_time", OracleDbType.Date).Value = booking.BookingTime;

                    cmd.ExecuteNonQuery();
                }
            }
        }



        public Booking get_booking_by_id(int bookingId)
        {
            Booking booking = null;

            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand("olerning.PKG_GIORGITSK_BOOKINGS.get_booking_by_id", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_id", OracleDbType.Int32).Value = bookingId;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            booking = new Booking
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                                Description = reader["description"] != DBNull.Value ? reader["description"].ToString() : null,
                                BookingTime = reader.GetDateTime(reader.GetOrdinal("booking_time"))
                            };
                        }
                    }
                }
            }
            return booking;
        }


        public List<Booking> get_by_userId(long userId)
        {
            List<Booking> bookings = new List<Booking>();

            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_GIORGITSK_BOOKINGS.get_by_userId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_user_id", OracleDbType.Int64).Value = userId;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookings.Add(new Booking
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                                Description = reader["description"].ToString(),
                                BookingTime = reader.GetDateTime(reader.GetOrdinal("booking_time"))
                            });
                        }
                    }
                }
            }

            return bookings;
        }



        public List<Booking> get_by_doctorId(long doctorId)
        {
            List<Booking> bookings = new List<Booking>();

            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_GIORGITSK_BOOKINGS.get_by_doctorId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_doctor_id", OracleDbType.Int64).Value = doctorId;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bookings.Add(new Booking
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                DoctorId = reader.GetInt32(reader.GetOrdinal("doctor_id")),
                                Description = reader["description"].ToString(),
                                BookingTime = reader.GetDateTime(reader.GetOrdinal("booking_time"))
                            });
                        }
                    }
                }
            }

            return bookings;
        }



    }
}
