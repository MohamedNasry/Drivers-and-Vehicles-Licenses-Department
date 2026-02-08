using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsUsersData
    {
        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName
            , ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * From Users where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())//تستعملها فالـ loop باش تقرأ الأسطر
                {
                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }

                

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }

                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }

            return isFound;

            //bool isFound = false;

            //string query = "SELECT * FROM Users WHERE UserID = @UserID";

            //using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            //using (SqlCommand command = new SqlCommand(query, connection))
            //{
            //    command.Parameters.AddWithValue("@UserID", UserID);

            //    try
            //    {
            //        connection.Open();

            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                isFound = true;

            //                PersonID = (int)reader["PersonID"];
            //                UserName = (string)reader["UserName"];
            //                Password = (string)reader["Password"];
            //                if ((byte)reader["IsActive"] == 1)
            //                {
            //                    IsActive = true;
            //                }
            //                else
            //                {
            //                    IsActive = false;
            //                }
            //            }
            //        } // هنا الـ reader يتسد أوتوماتيكياً
            //    }
            //    catch (Exception)
            //    {
            //        isFound = false;
            //    }
            //} // هنا الـ connection و command يتسدو أوتوماتيكياً

            //return isFound;
        }

        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string UserName,
             ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "select * From Users where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())//تستعملها فالـ loop باش تقرأ الأسطر
                {
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    if ((byte)reader["IsActive"] == 1)
                    {
                        IsActive = true;
                    }
                    else
                    {
                        IsActive = false;
                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetUserInfoByUserNameAndPassword(string UserName, string Password,
            ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From Users where 
                                        UserName = @UserName And Password = @Password";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            SqlDataReader reader = null;
            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())//تستعملها فالـ loop باش تقرأ الأسطر
                {
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];
                }

            

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    //reader.Dispose();
                }
                connection.Close();
            }

            return isFound;

        }

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @" SELECT 
                                    Users.UserID,
                                    Users.PersonID,
                                    People.FirstName 
                                        + ' ' + People.LastName 
                                        + ' ' + 
                                        CASE 
                                            WHEN People.ThirdName IS NULL THEN '' 
                                            ELSE People.ThirdName 
                                        END AS FullName,
                                    Users.UserName,
                                    Users.Password,
                                    Users.IsActive
                                            FROM Users
                                            INNER JOIN People 
                                                ON Users.PersonID = People.PersonID;";

            SqlCommand command = new SqlCommand(query,connection);


            SqlDataReader reader = null;

            try
            {
                connection.Open();

                 reader = command.ExecuteReader();

                if (reader.HasRows)//غير باش تتأكد واش النتيجة فارغة ولا لا
                {
                    dt.Load(reader);
                }

                //reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                   
                }


                connection.Close();
            }
            return dt;
        }


        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Users
                                        (PersonID,UserName,Password,IsActive)
                                         Values
                                    (@PersonID,@UserName,@Password,@IsActive);
                             select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            if (IsActive == true)
            {
                command.Parameters.AddWithValue("IsActive", 1);
            }
            else
            {

                command.Parameters.AddWithValue("IsActive", 0);
            }


                try
                {
                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        UserID = insertedID;
                    }

                }
                catch (Exception ex)
                {
                    UserID = -1;
                }
                finally
                {
                    connection.Close();
                }

            return UserID;

        }


        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Delete Users where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                rowsAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;

        }

        public static bool UpdateUser(int UserID,int PersonID, string UserName, 
            string Password, bool IsActive)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Users 
                                set PersonID = @PersonID,
                                    UserName = @UserName,
                                    Password = @Password,
                                    IsActive = @IsActive
                            Where UserID = @UserID;";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@UserID",UserID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            if (IsActive == true)
            {
                command.Parameters.AddWithValue("IsActive", 1);
            }
            else
            {

                command.Parameters.AddWithValue("IsActive", 0);
            }

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                rowsAffected = 0;
            }
            finally
            {
                connection.Close();

            }

            return rowsAffected > 0;
        }


        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select Found = 1 From Users where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }


        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select Found = 1 From Users where UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();

            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 from Users
                                                  where  PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool ChangePassword(int UserID, string newPassword)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Users
                                        set Password = @NewPassword
                                where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NewPassword", newPassword);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                rowsAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }


    }
}
