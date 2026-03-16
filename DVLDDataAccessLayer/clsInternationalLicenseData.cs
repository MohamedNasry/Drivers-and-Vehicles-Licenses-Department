using DVLD_Domain.DTO;
using DVLD_Loggin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsInternationalLicenseData
    {
        public static bool GetInternationalLicenseByID(clsInternationalLicenseDTO dto)
        {
            bool isFound = false;
            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@InternationalLicenseID", dto.InternationalLicenseID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            dto.ApplicantID = Convert.ToInt32(reader["ApplicationID"]);
                            dto.DriverID = Convert.ToInt32(reader["DriverID"]);
                            dto.IssuedUsingLocalLicenseID = Convert.ToInt32(reader["IssuedUsingLocalLicenseID"]);
                            dto.IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                            dto.ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);
                            dto.IsActive = Convert.ToBoolean(reader["IsActive"]);
                            dto.CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                            isFound = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetInternationalLicenseByID));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static int AddNewInternationalLicense(clsInternationalLicenseDTO dto)
        {
            int insertedID = -1;
            string query = @"
                UPDATE InternationalLicenses SET IsActive = 0 WHERE DriverID = @DriverID;
                INSERT INTO InternationalLicenses (ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID)
                VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID);
                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", dto.ApplicantID);
                command.Parameters.AddWithValue("@DriverID", dto.DriverID);
                command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", dto.IssuedUsingLocalLicenseID);
                command.Parameters.AddWithValue("@IssueDate", dto.IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", dto.ExpirationDate);
                command.Parameters.AddWithValue("@IsActive", dto.IsActive);
                command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                        insertedID = Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(AddNewInternationalLicense));
                    insertedID = -1;
                }
            }

            return insertedID;
        }

        public static bool UpdateInternationalLicense(clsInternationalLicenseDTO dto)
        {
            int rowsAffected = 0;
            string query = @"
                UPDATE InternationalLicenses
                SET ApplicationID = @ApplicationID,
                    DriverID = @DriverID,
                    IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                    IssueDate = @IssueDate,
                    ExpirationDate = @ExpirationDate,
                    IsActive = @IsActive,
                    CreatedByUserID = @CreatedByUserID
                WHERE InternationalLicenseID = @InternationalLicenseID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ApplicationID", dto.ApplicantID);
                command.Parameters.AddWithValue("@DriverID", dto.DriverID);
                command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", dto.IssuedUsingLocalLicenseID);
                command.Parameters.AddWithValue("@IssueDate", dto.IssueDate);
                command.Parameters.AddWithValue("@ExpirationDate", dto.ExpirationDate);
                command.Parameters.AddWithValue("@IsActive", dto.IsActive);
                command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserID);
                command.Parameters.AddWithValue("@InternationalLicenseID", dto.InternationalLicenseID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(UpdateInternationalLicense));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive
                FROM InternationalLicenses
                ORDER BY IsActive, ExpirationDate DESC";

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
                    EventLogger.LogError(ex, nameof(GetAllInternationalLicenses));
                    dt = null;
                }
            }

            return dt;
        }

        public static DataTable GetDriverInternationalLicenses(int driverID)
        {
            DataTable dt = new DataTable();
            string query = @"
                SELECT InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive
                FROM InternationalLicenses
                WHERE DriverID = @DriverID
                ORDER BY ExpirationDate DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DriverID", driverID);

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
                    EventLogger.LogError(ex, nameof(GetDriverInternationalLicenses));
                    dt = null;
                }
            }

            return dt;
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int driverID)
        {
            int internationalLicenseID = -1;
            string query = @"
                SELECT TOP 1 InternationalLicenseID
                FROM InternationalLicenses
                WHERE DriverID = @DriverID
                  AND GETDATE() BETWEEN IssueDate AND ExpirationDate
                ORDER BY ExpirationDate DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DriverID", driverID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                        internationalLicenseID = Convert.ToInt32(result);
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetActiveInternationalLicenseIDByDriverID));
                    internationalLicenseID = -1;
                }
            }

            return internationalLicenseID;
        }
    }
}