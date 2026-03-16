using DVLD_Domain.DAO;
using DVLD_Loggin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsTestData
    {
        public static bool GetTestInfoByID(int TestID, clsTestDTO TestDTO)
        {
            bool isFound = false;
            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestID", TestID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            TestDTO.TestID = TestID;
                            TestDTO.TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestDTO.TestResult = reader["TestResult"] != DBNull.Value ? Convert.ToByte(reader["TestResult"]) : (byte)0;
                            TestDTO.Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "";
                            TestDTO.CreatedByUserID = (int)reader["CreatedByUserID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetTestInfoByID));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int TestTypeID, int LicenseClassID, clsTestDTO TestDTO)
        {
            bool isFound = false;

            string query = @"
                SELECT TOP 1 T.TestID, T.TestAppointmentID, T.TestResult, T.Notes, T.CreatedByUserID
                FROM TestAppointments TA
                INNER JOIN Tests T ON T.TestAppointmentID = TA.TestAppointmentID
                INNER JOIN LocalDrivingLicenseApplications LDA ON LDA.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID
                INNER JOIN Applications A ON A.ApplicationID = LDA.ApplicationID
                WHERE A.ApplicantPersonID = @PersonID
                  AND LDA.LicenseClassID = @LicenseClassID
                  AND TA.TestTypeID = @TestTypeID
                ORDER BY TA.TestAppointmentID DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            TestDTO.TestID = (int)reader["TestID"];
                            TestDTO.TestAppointmentID = (int)reader["TestAppointmentID"];
                            TestDTO.TestResult = reader["TestResult"] != DBNull.Value ? Convert.ToByte(reader["TestResult"]) : (byte)0;
                            TestDTO.Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "";
                            TestDTO.CreatedByUserID = (int)reader["CreatedByUserID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetLastTestByPersonAndTestTypeAndLicenseClass));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM Tests ORDER BY TestID";

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
                    EventLogger.LogError(ex, nameof(GetAllTests));
                    dt = null;
                }
            }

            return dt;
        }

        public static int AddNewTest(clsTestDTO TestDTO)
        {
            int TestID = -1;

            string query = @"
                INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
                VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);

                UPDATE TestAppointments
                SET IsLocked = 1
                WHERE TestAppointmentID = @TestAppointmentID;

                SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestAppointmentID", TestDTO.TestAppointmentID);
                command.Parameters.Add("@TestResult", SqlDbType.TinyInt).Value = TestDTO.TestResult;
                command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(TestDTO.Notes) ? (object)DBNull.Value : TestDTO.Notes);
                command.Parameters.AddWithValue("@CreatedByUserID", TestDTO.CreatedByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        TestID = insertedID;
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(AddNewTest));
                    TestID = -1;
                }
            }

            return TestID;
        }

        public static bool UpdateTest(clsTestDTO TestDTO)
        {
            int rowsAffected = 0;
            string query = @"
                UPDATE Tests
                SET TestAppointmentID = @TestAppointmentID,
                    TestResult = @TestResult,
                    Notes = @Notes,
                    CreatedByUserID = @CreatedByUserID
                WHERE TestID = @TestID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestID", SqlDbType.Int).Value = TestDTO.TestID;
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestDTO.TestAppointmentID;
                command.Parameters.Add("@TestResult", SqlDbType.TinyInt).Value = TestDTO.TestResult;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = TestDTO.CreatedByUserID;
                command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(TestDTO.Notes) ? (object)DBNull.Value : TestDTO.Notes);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(UpdateTest));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte count = 0;
            string query = @"
                SELECT COUNT(*) AS PassedTestCount
                FROM TestAppointments TA
                INNER JOIN Tests T ON T.TestAppointmentID = TA.TestAppointmentID
                WHERE TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                  AND T.TestResult = 1";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                        count = Convert.ToByte(result);
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetPassedTestCount));
                    count = 0;
                }
            }

            return count;
        }
    }
}