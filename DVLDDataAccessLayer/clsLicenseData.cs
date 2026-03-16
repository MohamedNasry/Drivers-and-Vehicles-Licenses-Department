using DVLD_Domain.DTO;
using DVLD_Loggin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsLicenseData
    {
        public static bool GetLicenseInfoByLicenseID(clsLicenseDTO dto)
        {
            bool isFound = false;
            string query = @"SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
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
                            dto.Notes = reader["Notes"] == DBNull.Value ? "" : reader["Notes"].ToString();
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
                    EventLogger.LogError(ex, nameof(GetLicenseInfoByLicenseID));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT * FROM Licenses";

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
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetAllLicenses));
                    dt = null;
                }
            }

            return dt;
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT L.LicenseID, L.ApplicationID, LC.ClassName,
                       L.IssueDate, L.ExpirationDate, L.IsActive
                FROM Licenses L
                INNER JOIN LicenseClasses LC ON L.LicenseClass = LC.LicenseClassID
                WHERE L.DriverID = @DriverID
                ORDER BY L.IsActive DESC, L.ExpirationDate DESC;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
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
                    EventLogger.LogError(ex, nameof(GetDriverLicenses));
                    dt = null;
                }
            }

            return dt;
        }

        public static int AddNewLicense(clsLicenseDTO dto)
        {
            int insertedID = -1;
            string query = @"
                INSERT INTO Licenses
                    (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                VALUES
                    (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", dto.ApplicationID);
                command.Parameters.AddWithValue("@DriverID", dto.DriverID);
                command.Parameters.AddWithValue("@LicenseClass", dto.LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", dto.IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", dto.ExpirationDate);
                command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value =
                    string.IsNullOrWhiteSpace(dto.Notes) ? (object)DBNull.Value : dto.Notes;
                command.Parameters.AddWithValue("@PaidFees", dto.PaidFees);
                command.Parameters.AddWithValue("@IsActive", dto.IsActive);
                command.Parameters.AddWithValue("@IssueReason", dto.IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                        insertedID = id;
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(AddNewLicense));
                    insertedID = -1;
                }
            }

            return insertedID;
        }

        public static bool UpdateLicense(clsLicenseDTO dto)
        {
            int rowsAffected = 0;
            string query = @"
                UPDATE Licenses
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
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", dto.ApplicationID);
                command.Parameters.AddWithValue("@DriverID", dto.DriverID);
                command.Parameters.AddWithValue("@LicenseClass", dto.LicenseClass);
                command.Parameters.AddWithValue("@IssueDate", dto.IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", dto.ExpirationDate);
                command.Parameters.Add("@Notes", SqlDbType.NVarChar).Value =
                    string.IsNullOrWhiteSpace(dto.Notes) ? (object)DBNull.Value : dto.Notes;
                command.Parameters.AddWithValue("@PaidFees", dto.PaidFees);
                command.Parameters.AddWithValue("@IsActive", dto.IsActive);
                command.Parameters.AddWithValue("@IssueReason", dto.IssueReason);
                command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                command.Parameters.AddWithValue("@LicenseID", dto.LicenseID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(UpdateLicense));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;
            string query = @"
                SELECT L.LicenseID
                FROM Licenses L
                INNER JOIN Drivers D ON L.DriverID = D.DriverID
                WHERE D.PersonID = @PersonID
                  AND L.LicenseClass = @LicenseClass
                  AND L.IsActive = 1;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int id))
                        LicenseID = id;
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetActiveLicenseIDByPersonID));
                    LicenseID = -1;
                }
            }

            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;
            string query = @"UPDATE Licenses SET IsActive = 0 WHERE LicenseID = @LicenseID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
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
                    EventLogger.LogError(ex, nameof(DeactivateLicense));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }
    }
}