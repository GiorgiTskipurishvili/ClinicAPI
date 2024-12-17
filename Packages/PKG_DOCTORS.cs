using ClinicAPI.DTOs;
using ClinicAPI.Models;
using ClinicAPI.Package;
using ClinicAPI.Services;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;

namespace ClinicAPI.Packages
{
    public class PKG_DOCTORS : PKG_BASE
    {


        private readonly IPdfService _pdfService;

        public PKG_DOCTORS(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }



        public void add_doctor(DoctorDTOs doctorDtos)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_DOCTORS.add_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = doctorDtos.FirstName;
            cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = doctorDtos.LastName;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = doctorDtos.Email;
            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = doctorDtos.Password;
            cmd.Parameters.Add("p_personal_id", OracleDbType.Int32).Value = doctorDtos.PersonalId;
            //cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = doctorDtos.Category;
            cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = doctorDtos.Category;
            cmd.Parameters.Add("p_photo", OracleDbType.Blob).Value = GetFileBytes(doctorDtos.Photo);
            cmd.Parameters.Add("p_cv", OracleDbType.Blob).Value = GetFileBytes(doctorDtos.Cv);
            cmd.Parameters.Add("p_role", OracleDbType.Int32).Value = doctorDtos.Role;
            cmd.Parameters.Add("p_rating", OracleDbType.Double).Value = doctorDtos.Rating;


            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private byte[]? GetFileBytes(IFormFile? file)
        {
            if (file == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }



        public List<Doctor> get_doctors()
        {
            List<Doctor> doctors = new List<Doctor>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_DOCTORS.get_doctors";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Doctor doctor = new Doctor()
                {
                    Id = Convert.ToInt64(reader["id"]),
                    FirstName = reader["first_name"].ToString(),
                    LastName = reader["last_name"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    PersonalId = Convert.ToInt64(reader["personal_id"]),
                    Category = reader["category"].ToString(),
                    Role = Convert.ToInt32(reader["role"]),
                    Rating = Convert.ToDouble(reader["rating"]),
                    Photo = reader["photo"] != DBNull.Value ? (byte[])reader["photo"] : null,  // Check for null
                    Cv = reader["cv"] != DBNull.Value ? (byte[])reader["cv"] : null
                };
                doctors.Add(doctor);
            }
            conn.Close();
            return doctors;
        }








        public void delete_doctor(Doctor doctor)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();


            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_DOCTORS.delete_doctor";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int64).Value = doctor.Id;

            cmd.ExecuteNonQuery();
            conn.Close();
        }




        public void update_doctor(Doctor doctor)
        {

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();


            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_DOCTORS.update_doctor";
            cmd.CommandType = CommandType.StoredProcedure;



            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int64).Value = doctor.Id;
            cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = doctor.FirstName;
            cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = doctor.LastName;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = doctor.Email;
            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = doctor.Password;
            cmd.Parameters.Add("p_personal_id", OracleDbType.Int64).Value = doctor.PersonalId;
            cmd.Parameters.Add("p_category", OracleDbType.Varchar2).Value = doctor.Category;
            cmd.Parameters.Add("p_role", OracleDbType.Int32).Value = doctor.Role;
            cmd.Parameters.Add("p_rating", OracleDbType.Double).Value = doctor.Rating;

            cmd.ExecuteNonQuery();

            conn.Close();

        }




        public Doctor get_doctor_by_id(Doctor doctor)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_DOCTORS.get_doctor_by_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int64).Value = doctor.Id;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                doctor.Id = Convert.ToInt64(reader["id"]);
                doctor.FirstName = reader["first_name"].ToString();
                doctor.LastName = reader["last_name"].ToString();
                doctor.Email = reader["email"].ToString();
                doctor.Password = reader["password"].ToString();
                doctor.PersonalId = Convert.ToInt64(reader["personal_id"]);
                doctor.Category = reader["category"].ToString();
                doctor.Role = Convert.ToInt32(reader["role"]);
                doctor.Rating = Convert.ToDouble(reader["rating"]);
                doctor.Photo = reader["photo"] != DBNull.Value ? (byte[])reader["photo"] : null;
                doctor.Cv = reader["cv"] != DBNull.Value ? (byte[])reader["cv"] : null;

            }
            else
            {
                return null;
            }

            conn.Close();
            return doctor;
        }





        public Doctor get_doctor_by_email(string email)
        {
            Doctor doctor = null;

            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_GIORGITSK_DOCTORS.get_doctor_by_email", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            doctor = new Doctor
                            {
                                Id = long.Parse(reader["id"].ToString()),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                PersonalId = long.Parse(reader["personal_id"].ToString()),
                                Category = reader["category"].ToString(),
                                Role = int.Parse(reader["role"].ToString()),
                                Rating = double.Parse(reader["rating"].ToString()),
                                Photo = reader["photo"] != DBNull.Value ? (byte[])reader["photo"] : null,
                                Cv = reader["cv"] != DBNull.Value ? (byte[])reader["cv"] : null
                            };
                        }
                    }
                }
            }
            return doctor;
        }





        public byte[]? get_cv_by_doctor_id(int doctorId)
        {
            using (var connection = new OracleConnection(ConnStr))
            {
                connection.Open();

                using (var command = new OracleCommand("olerning.PKG_GIORGITSK_DOCTORS.get_cv_by_doctor_id", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.BindByName = true; // Ensure parameters are bound by name

                    // Input parameter
                    var p_id_param = new OracleParameter("p_id", OracleDbType.Decimal)
                    {
                        Direction = ParameterDirection.Input,
                        Value = doctorId
                    };
                    command.Parameters.Add(p_id_param);

                    // Output parameter
                    var resultParam = new OracleParameter("p_result", OracleDbType.Blob)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(resultParam);

                    command.ExecuteNonQuery();

                    // Check and return the result
                    if (resultParam.Value != DBNull.Value)
                    {
                        using (OracleBlob blob = (OracleBlob)resultParam.Value)
                        {
                            byte[] blobData = new byte[blob.Length];
                            blob.Read(blobData, 0, (int)blob.Length);
                            return blobData;
                        }
                    }
                }
            }

            return null;
        }


        public async Task<string> ExtractAndStoreCvTextAsync(int doctorId)
        {
            // Fetch the CV (PDF) from the database
            var pdfData = get_cv_by_doctor_id(doctorId);

            if (pdfData == null || pdfData.Length == 0)
            {
                throw new InvalidOperationException("No CV found for the specified doctor ID.");
            }

            // Extract the text from the PDF using PdfService
            var extractedText = await _pdfService.ExtractTextFromPdfAsync(pdfData);

            // Store the extracted text back into the database

            return extractedText;
        }


    }
}
