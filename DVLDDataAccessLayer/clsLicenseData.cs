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
    public class clsLicenseData
    {

        public static bool GetLicenseInfoByLicenseID(clsLicenseDTO dto)
        {
            bool isFound = false;

            string query = @"Select * From Licenses Where LicenseID = @LicenseID";

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
                                dto.ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                                dto.DriverID = Convert.ToInt32(reader["DriverID"]);
                                dto.LicenseClass = Convert.ToInt32(reader["LicenseClass"]);
                                dto.IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                                dto.ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);

                                if (reader["Notes"] != DBNull.Value)
                                {
                                    dto.Notes = reader["Notes"].ToString();
                                }
                                else
                                {
                                    dto.Notes = "";
                                }


                                dto.PaidFees = Convert.ToDouble(reader["PaidFees"]);
                                dto.IsActive = Convert.ToBoolean(reader["IsActive"]);
                                dto.IssueReason = Convert.ToInt32(reader["IssueReason"]);
                                dto.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                                isFound = true;
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
        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            string query = @"Select * From Licenses";
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
        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
                        L.LicenseID, L.ApplicationID, LC.ClassName,
                        L.IssueDate, L.ExpirationDate, L.IsActive
                     FROM Licenses L
                     INNER JOIN LicenseClasses LC
                        ON L.LicenseClass = LC.LicenseClassID
                     WHERE L.DriverID = @DriverID
                     ORDER BY L.IsActive DESC, L.ExpirationDate DESC;";

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

        public static int AddNewLicense(clsLicenseDTO dto)
        {
            int insertedID = -1;
            string query = @"INSERT INTO Licenses
                             (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                             VALUES
                             (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", dto.ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", dto.DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", dto.LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", dto.IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", dto.ExpirationDate);
                    //command.Parameters.AddWithValue("@Notes", dto.Notes ?? (object)DBNull.Value);
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = string.IsNullOrWhiteSpace(dto.Notes)
                                                                                                    ? (object)DBNull.Value
                                                                                                    : dto.Notes;

                    command.Parameters.AddWithValue("@PaidFees", dto.PaidFees);
                    command.Parameters.AddWithValue("@IsActive", dto.IsActive);
                    command.Parameters.AddWithValue("@IssueReason", (int)dto.IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            insertedID = id;
                        }
                    }
                    catch (Exception ex)
                    {
                        insertedID = -1;
                    }
                }
            }
            return insertedID;
        }

        public static bool UpdateLicense(clsLicenseDTO dto)
        {
            int rowsAffected = 0;
            string query = @"UPDATE Licenses
                             SET ApplicationID = @ApplicationID,
                                 DriverID = @DriverID,
                                 LicenseClass = @LicenseClass,
                                 IssueDate = @IssueDate,
                                 ExpirationDate = @ExpirationDate,
                                 Notes = @Notes,
                                 PaidFees = @PaidFees,
                                 IsActive = @IsActive,
                                 IssueReason = @IssueReason,
                                 CreatedByUserID = @CreatedByUserID
                             WHERE LicenseID = @LicenseID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", dto.ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", dto.DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", dto.LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", dto.IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", dto.ExpirationDate);
       
                    command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = string.IsNullOrWhiteSpace(dto.Notes)
                                                                                                    ? (object)DBNull.Value
                                                                                                    : dto.Notes;
                    command.Parameters.AddWithValue("@PaidFees", dto.PaidFees);
                    command.Parameters.AddWithValue("@IsActive", dto.IsActive);
                    command.Parameters.AddWithValue("@IssueReason", (int)dto.IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                    command.Parameters.AddWithValue("@LicenseID", dto.LicenseID);
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

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT        Licenses.LicenseID
                            FROM Licenses INNER JOIN
                                                     Drivers ON Licenses.DriverID = Drivers.DriverID
                            WHERE  
                             
                             Licenses.LicenseClass = @LicenseClass 
                              AND Drivers.PersonID = @PersonID
                              And IsActive=1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;
            string query = @"UPDATE Licenses
                             SET IsActive = 0
                             WHERE LicenseID = @LicenseID;";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
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
