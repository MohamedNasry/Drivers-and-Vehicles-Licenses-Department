using DVLD_Domain.DTO;
using DVLD_Loggin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsDriversData
    {
        public static bool GetDriverInfoByID(int DriverID, clsDriverDTO dto)
        {
            bool isFound = false;
            string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DriverID", DriverID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dto.DriverId = DriverID;
                            dto.PersonId = Convert.ToInt32(reader["PersonID"]);
                            dto.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            dto.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                            isFound = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetDriverInfoByID));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, clsDriverDTO dto)
        {
            bool isFound = false;
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

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
                            dto.DriverId = Convert.ToInt32(reader["DriverID"]);
                            dto.PersonId = PersonID;
                            dto.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            dto.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                            isFound = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetDriverInfoByPersonID));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Drivers_View ORDER BY DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dt.Load(reader);
                        else
                            dt = null;
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetAll));
                    dt = null;
                }
            }

            return dt;
        }

        public static int AddNewDriver(clsDriverDTO dto)
        {
            int newDriverID = -1;
            string query = @"
                INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", dto.PersonId);
                command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                        newDriverID = Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(AddNewDriver));
                    newDriverID = -1;
                }
            }

            return newDriverID;
        }

        public static bool UpdateDriver(int DriverID, clsDriverDTO dto)
        {
            int rowsAffected = 0;
            string query = @"
                UPDATE Drivers
                SET PersonID = @PersonID,
                    CreatedByUserID = @CreatedByUserID
                WHERE DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DriverID", DriverID);
                command.Parameters.AddWithValue("@PersonID", dto.PersonId);
                command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(UpdateDriver));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }

        public static bool DeleteDriver(int DriverID)
        {
            int rowsAffected = 0;
            string query = "DELETE FROM Drivers WHERE DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DriverID", DriverID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(DeleteDriver));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }
    }
}