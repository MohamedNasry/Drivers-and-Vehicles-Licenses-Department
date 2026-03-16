using DVLD_Loggin;
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
   

  

   
        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            PersonID = reader["PersonID"] != DBNull.Value ? (int)reader["PersonID"] : -1;
                            UserName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : "";
                            Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : "";
                            IsActive = reader["IsActive"] != DBNull.Value && (bool)reader["IsActive"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetUserInfoByUserID));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE PersonID = @PersonID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            UserID = reader["UserID"] != DBNull.Value ? (int)reader["UserID"] : -1;
                            UserName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : "";
                            Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : "";
                            IsActive = reader["IsActive"] != DBNull.Value && (bool)reader["IsActive"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetUserInfoByPersonID));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetUserInfoByUserNameAndPassword(string UserName, string Password, ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            UserID = (int)reader["UserID"];
                            PersonID = (int)reader["PersonID"];
                            Password = (string)reader["Password"];
                            IsActive = (bool)reader["IsActive"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetUserInfoByUserNameAndPassword));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
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

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetAllUsers));
                    dt = null;
                }
            }

            return dt;
        }



        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;
            string query = @"
                INSERT INTO Users (PersonID, UserName, Password, IsActive)
                VALUES (@PersonID, @UserName, @Password, @IsActive);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@IsActive", IsActive ? 1 : 0);

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
                    EventLogger.LogError(ex, nameof(AddNewUser));
                    UserID = -1;
                }
            }

            return UserID;
        }



        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;
            string query = "DELETE FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(DeleteUser));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }


        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            int rowsAffected = 0;
            string query = @"
                UPDATE Users
                SET PersonID = @PersonID,
                    UserName = @UserName,
                    Password = @Password,
                    IsActive = @IsActive
                WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@UserName", UserName);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@IsActive", IsActive ? 1 : 0);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(UpdateUser));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }



        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;
            string query = "SELECT found = 1 FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserID", UserID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        isFound = reader.HasRows;
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(IsUserExist));
                    isFound = false;
                }
            }

            return isFound;
        }


        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;
            string query = "SELECT found = 1 FROM Users WHERE UserName = @UserName";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", UserName);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        isFound = reader.HasRows;
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(IsUserExist));
                    isFound = false;
                }
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
            string query = "UPDATE Users SET Password = @NewPassword WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NewPassword", newPassword);
                command.Parameters.AddWithValue("@UserID", UserID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(ChangePassword));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }


    }
}
