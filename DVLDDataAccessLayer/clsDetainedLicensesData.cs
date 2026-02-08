using DVLD_Domain.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsDetainedLicensesData
    {
        public static bool GetDetainedLicenseInfoByID(clsDetainedDTO dto)
        {
            bool isFound = false;
            string query = "Select * from DetainedLicenses where DetainID = @DetainID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", dto.DetainID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                dto.LicenseID = Convert.ToInt32(reader["LicenseID"]);
                                dto.DetainDate = Convert.ToDateTime(reader["DetainDate"]);
                                dto.FineFees = Convert.ToDouble(reader["FineFees"]);
                                dto.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                                dto.IsReleased = Convert.ToBoolean(reader["IsReleased"]);
                                dto.ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(reader["ReleaseDate"]);
                                dto.ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : Convert.ToInt32(reader["ReleasedByUserID"]);
                                dto.ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : Convert.ToInt32(reader["ReleaseApplicationID"]);
                                isFound = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        //throw new Exception("An error occurred while retrieving detained license information.", ex);
                        isFound = false;

                    }
                }
            }
            return isFound;
        }


        public static bool GetDetainedLicenseInfoByLicenseID(clsDetainedDTO dto)
        {
            bool isFound = false;
            string query = "Select * from DetainedLicenses where LicenseID = @LicenseID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", dto.LicenseID);
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dto.DetainID = Convert.ToInt32(reader["DetainID"]);
                                dto.DetainDate = Convert.ToDateTime(reader["DetainDate"]);
                                dto.FineFees = Convert.ToDouble(reader["FineFees"]);
                                dto.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                                dto.IsReleased = Convert.ToBoolean(reader["IsReleased"]);
                                dto.ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MaxValue : Convert.ToDateTime(reader["ReleaseDate"]);
                                dto.ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : Convert.ToInt32(reader["ReleasedByUserID"]);
                                dto.ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : Convert.ToInt32(reader["ReleaseApplicationID"]);
                                isFound = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("An error occurred while retrieving detained license information.", ex);
                        isFound = false;
                    }
                }
            }
            return isFound;

        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dtDetainedLicenses = new DataTable();
            string query = "Select * from DetainedLicenses_View order by isReleased,DetainID";
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
                            {
                                dtDetainedLicenses.Load(reader);
                            }
                            else
                            {
                                dtDetainedLicenses = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("An error occurred while retrieving all detained licenses.", ex);
                        dtDetainedLicenses = null;
                    }
                }
            }
            return dtDetainedLicenses;
        }

        public static int AddNewDetainedLicense(clsDetainedDTO dto)
        {
            int newDetainID = -1;
            string query = @"Insert into DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, isReleased) 
                           Values (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0); 
                           SELECT SCOPE_IDENTITY();";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", dto.LicenseID);
                    command.Parameters.AddWithValue("@DetainDate", dto.DetainDate);
                    command.Parameters.AddWithValue("@FineFees", dto.FineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            newDetainID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("An error occurred while adding a new detained license.", ex);
                        newDetainID = -1;
                    }
                }
            }
            return newDetainID;
        }


        public static bool UpdateDetainedLicense(clsDetainedDTO dto)
        {
            int rowsAffected = 0;

            string query = @"Update DetainedLicenses
                                set LicenseID = @LicenseID , DetainDate = @DetainDate, FineFees = @FineFees, CreatedByUserID = @CreatedByUserID, IsReleased = @IsReleased
                            Where DetainID = @DetainID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", dto.LicenseID);
                    command.Parameters.AddWithValue("@DetainDate", dto.DetainDate);
                    command.Parameters.AddWithValue("@FineFees", dto.FineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                    command.Parameters.AddWithValue("@IsReleased", dto.IsReleased);
                    command.Parameters.AddWithValue("@DetainID", dto.DetainID);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("An error occurred while updating the detained license.", ex);
                        rowsAffected = 0;
                    }
                }
            }
            return rowsAffected > 0;
        }

        public static bool ReleaseDetainedLicense(clsDetainedDTO dto)
        {
            int rowsAffected = 0;
            string query = @"Update DetainedLicenses
                                set IsReleased = 1, ReleaseDate = @ReleaseDate, ReleasedByUserID = @ReleasedByUserID, ReleaseApplicationID = @ReleaseApplicationID
                            Where DetainID = @DetainID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                    command.Parameters.AddWithValue("@ReleasedByUserID", dto.ReleasedByUserID);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", dto.ReleaseApplicationID);
                    command.Parameters.AddWithValue("@DetainID", dto.DetainID);
                    try
                    {
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("An error occurred while releasing the detained license.", ex);
                        rowsAffected = 0;
                    }
                }
            }
            return rowsAffected > 0;

        }


        public static bool isLicenseDetained(int LicenseID)
        {
            bool isDetained = false;
            string query = "Select isDetained = 1 from DetainedLicenses where LicenseID = @LicenseID and IsReleased = 0";

            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            isDetained = Convert.ToBoolean(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        //throw new Exception("An error occurred while checking if the license is detained.", ex);
                        isDetained = false;
                    }
                }
            }
            return isDetained;
        }

    }
}
