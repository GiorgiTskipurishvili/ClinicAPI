using ClinicAPI.DTOs;
using ClinicAPI.Models;
using ClinicAPI.Package;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClinicAPI.Packages
{
    public class PKG_USERS:PKG_BASE
    {
        public void add_user(UserDTOs userDtos)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_USERS.add_user";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = userDtos.FirstName;
            cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = userDtos.LastName;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = userDtos.Email;
            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = userDtos.Password;
            cmd.Parameters.Add("p_personal_id", OracleDbType.Int32).Value = userDtos.PersonalId;
            cmd.Parameters.Add("p_role", OracleDbType.Int32).Value = userDtos.Role;

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        //public void add_user(User user)
        //{
        //    OracleConnection conn = new OracleConnection();
        //    conn.ConnectionString = ConnStr;
        //    conn.Open();

        //    OracleCommand cmd = new OracleCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = "olerning.PKG_GIORGITSK_USERS.add_user";
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = user.FirstName;
        //    cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = user.LastName;
        //    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = user.Email;
        //    cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = user.Password;
        //    cmd.Parameters.Add("p_personal_id", OracleDbType.Int32).Value = user.PersonalId;
        //    cmd.Parameters.Add("p_role", OracleDbType.Int32).Value = user.Role;

        //    cmd.ExecuteNonQuery();

        //    conn.Close();
        //}


        public List<User> get_users()
        {
            List<User> users = new List<User>();

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;
            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_USERS.get_users";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                User user = new User()
                {
                    Id = int.Parse(reader["id"].ToString()),
                    FirstName = reader["first_name"].ToString(),
                    LastName = reader["last_name"].ToString(),
                    Email = reader["email"].ToString(),
                    Password = reader["password"].ToString(),
                    PersonalId = int.Parse(reader["personal_id"].ToString()),
                    Role = int.Parse(reader["role"].ToString())
                };

                //user.FirstName = reader.GetString(1);

                users.Add(user);
            }


            conn.Close();
            return users;
        }
            


        public void delete_user(User user)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();


            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_USERS.delete_user";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int64).Value = user.Id;

            cmd.ExecuteNonQuery();
            conn.Close();
        }


        public void update_user(User user)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_USERS.update_user";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int64).Value = user.Id;
            cmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Value = user.FirstName;
            cmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Value = user.LastName;
            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = user.Email;
            cmd.Parameters.Add("p_password", OracleDbType.Varchar2).Value = user.Password;
            cmd.Parameters.Add("p_personal_id", OracleDbType.Int32).Value = user.PersonalId;
            cmd.Parameters.Add("p_role", OracleDbType.Int32).Value = user.Role;

            cmd.ExecuteNonQuery();
        }
        


        public User get_user_by_id(User user)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = ConnStr;

            conn.Open();

            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "olerning.PKG_GIORGITSK_USERS.get_user_by_id";
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("p_id", OracleDbType.Int64).Value = user.Id;
            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                user.Id = int.Parse(reader["id"].ToString());
                user.FirstName = reader["first_name"].ToString();
                user.LastName = reader["last_name"].ToString();
                user.Email = reader["email"].ToString();
                user.Password = reader["password"].ToString();
                user.PersonalId = int.Parse(reader["personal_id"].ToString());
                user.Role = int.Parse(reader["role"].ToString());
            }
            else
            {
                return null;
            }

            conn.Close();
            return user;
        }


        //public User GetUserByEmail(string email)
        //{
        //    User user = null;
        //    using (OracleConnection conn = new OracleConnection(ConnStr))
        //    {
        //        conn.Open();
        //        using (OracleCommand cmd = new OracleCommand("PKG_GIORGITSK_USERS.get_user_by_email", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;


        //            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
        //            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        //            try
        //            {
        //                using (OracleDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        user = new User
        //                        {
        //                            Id = reader.GetInt32("id"),
        //                            FirstName = reader.GetString("first_name"),
        //                            LastName = reader.GetString("last_name"),
        //                            Email = reader.GetString("email"),
        //                            Password = reader.GetString("password"),
        //                            PersonalId = reader.GetInt32("personal_id"),
        //                            Role = Convert.ToInt32(reader["role"])
        //                        };
        //                    }
        //                }
        //            }
        //            catch (OracleException ex)
        //            {
        //                Console.WriteLine($"Oracle Error: {ex.Message}");
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine($"Error: {ex.Message}");
        //            }
        //        }
        //    }
        //    return user;
        //}

        //public User GetUserByEmail(string email)
        //{
        //    User user = null;

        //    using (OracleConnection conn = new OracleConnection(ConnStr))
        //    {
        //        conn.Open();

        //        using (OracleCommand cmd = new OracleCommand("olerning.PKG_USER.get_user_by_email", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            // Input parameter for email
        //            cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
        //            // Output parameter for result cursor
        //            cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        //            using (OracleDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    user = new User
        //                    {
        //                        Id = int.Parse(reader["id"].ToString()),
        //                        FirstName = reader["first_name"].ToString(),
        //                        LastName = reader["last_name"].ToString(),
        //                        Email = reader["email"].ToString(),
        //                        Password = reader["password"].ToString(),
        //                        PersonalId = int.Parse(reader["personal_id"].ToString()),
        //                        Role = int.Parse(reader["role"].ToString())
        //                    };
        //                }
        //            }
        //        }
        //    }

        //    return user;
        //}



        public User get_user_by_email(string email)
        {
            User user = null;

            using (OracleConnection conn = new OracleConnection(ConnStr))
            {
                conn.Open();

                using (OracleCommand cmd = new OracleCommand("olerning.PKG_GIORGITSK_USERS.get_user_by_email", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("p_email", OracleDbType.Varchar2).Value = email;
                    cmd.Parameters.Add("p_result", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = int.Parse(reader["id"].ToString()),
                                FirstName = reader["first_name"].ToString(),
                                LastName = reader["last_name"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                PersonalId = int.Parse(reader["personal_id"].ToString()),
                                Role = int.Parse(reader["role"].ToString())
                            };
                        }
                    }
                }
            }
            return user;
        }



    }
}
