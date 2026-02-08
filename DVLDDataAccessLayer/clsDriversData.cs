using DVLD_Domain.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsDriversData
    {

        public static bool GetDriverInfoByID(int DriverID, clsDriverDTO dto)
        {
            bool isFound = false;

            string query = "Select * From Drivers where DriverID = @DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
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
                                isFound = true;

                                dto.DriverId = DriverID;
                                dto.PersonId = (int)reader["PersonID"];
                                dto.CreatedByUserID = (int)reader["CreatedByUserID"];
                                dto.CreatedDate = (DateTime)reader["CreatedDate"];

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, clsDriverDTO dto)
        {
            bool isFound = false;
            string query = "Select * From Drivers where PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
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
                                dto.DriverId = (int)reader["DriverID"];
                                dto.PersonId = PersonID;
                                dto.CreatedByUserID = (int)reader["CreatedByUserID"];
                                dto.CreatedDate = (DateTime)reader["CreatedDate"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }
        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();

            string query = "Select * From Drivers_View order by DriverID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);

                        }

                    }
                    catch (Exception ex)
                    {
                        dt = null;
                    }
                }
            }
            return dt;

        }

        public static int AddNewDriver(clsDriverDTO dto)
        {
            int newDriverID = -1;
            string query = @"Insert into Drivers (PersonID, CreatedByUserID, CreatedDate)
                           Values (@PersonID, @CreatedByUserID, @CreatedDate); 
                           Select SCOPE_IDENTITY();";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
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
                        {
                            newDriverID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        newDriverID = -1;
                    }
                }
            }
            return newDriverID;

        }

        public static bool UpdateDriver(int DriverID, clsDriverDTO dto)
        {
            int rowsAffected = 0;
            string query = @"Update Drivers 
                             Set PersonID = @PersonID,
                                 CreatedByUserID = @CreatedByUserID
                             Where DriverID = @DriverID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
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
                        rowsAffected = 0;
                    }
                }
            }
            return rowsAffected > 0;
        }

        public static bool DeleteDriver(int DriverID)
        {
            int rowsAffected = 0;
            string query = @"Delete From Drivers Where DriverID = @DriverID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
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
                        rowsAffected = 0;
                    }
                }
            }
            return rowsAffected > 0;
        }

    }
}
