using DVLD_Domain.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsTestData
    {

        public static bool GetTestInfoByID (int TestID,clsTestDTO TestDTO)
        {
            bool isFound = false;

            string query = "Select * From Tests Where TestID = @TestID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
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
                                TestDTO.TestResult = reader["TestResult"] == DBNull.Value ? (byte)0 : Convert.ToByte(reader["TestResult"]);

                                if (reader["Notes"] == DBNull.Value)
                                {
                                    TestDTO.Notes = "";
                                }
                                else
                                        TestDTO.Notes = (string)reader["Notes"];
                                    
                                TestDTO.CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                    catch
                    {
                        isFound = false;
                    }

                }
            }

            return isFound;

        }

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int PersonID, int TestTypeID, int LicenseClassID, clsTestDTO TestDTO)
        {
            bool isFound = false;
            string query = @"SELECT TOP 1
                                       T.TestID,
                                       T.TestAppointmentID,
                                       T.TestResult,
                                       T.Notes,
                                       T.CreatedByUserID
                                    
                                FROM TestAppointments TA
                                INNER JOIN Tests T
                                        ON T.TestAppointmentID = TA.TestAppointmentID
                                INNER JOIN LocalDrivingLicenseApplications LDA
                                        ON LDA.LocalDrivingLicenseApplicationID = TA.LocalDrivingLicenseApplicationID
                                INNER JOIN Applications A
                                        ON A.ApplicationID = LDA.ApplicationID
                                WHERE A.ApplicantPersonID = @PersonID
                                  AND LDA.LicenseClassID = @LicenseClassID
                                  AND TA.TestTypeID = @TestTypeID
                                ORDER BY TA.TestAppointmentID DESC;
";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
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
                                TestDTO.TestResult = reader["TestResult"] == DBNull.Value ? (byte)0 : Convert.ToByte(reader["TestResult"]);
                                if (reader["Notes"] == DBNull.Value)
                                {
                                    TestDTO.Notes = "";
                                }
                                else
                                    TestDTO.Notes = (string)reader["Notes"];
                                TestDTO.CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                        }
                    }
                    catch
                    {
                        isFound = false;
                    }
                }
            }
            return isFound;
        }
        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();

            string query = "Select * From Tests Order by TestID";

            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query,connection))
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
                        dt = null;
                    }
                }
            }

            return dt;
        }


        public static int AddNewTest(clsTestDTO TestDTO)
        {
            int TestID = -1;

            string query = @"Insert into Tests (TestAppointmentID,TestResult,Notes,CreatedByUserID) 
                                        Values (@TestAppointmentID,@TestResult,@Notes,@CreatedByUserID)
                             
                             Update TestAppointments 
                                Set IsLocked = 1
                                Where TestAppointmentID = @TestAppointmentID;   
                             Select SCOPE_IDENTITY();";

            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", TestDTO.TestAppointmentID);
                    command.Parameters.Add("@TestResult", SqlDbType.TinyInt).Value = TestDTO.TestResult;
            
                    if (string.IsNullOrEmpty(TestDTO.Notes))
                    {
                        command.Parameters.AddWithValue("@Notes", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Notes", TestDTO.Notes);
                    }


                    command.Parameters.AddWithValue("@CreatedByUserID", TestDTO.CreatedByUserID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null)
                        TestID = Convert.ToInt32(result);
                }
            }

            return TestID;

        }

        public static bool UpdateTest(clsTestDTO TestDTO)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Tests
                     SET TestAppointmentID = @TestAppointmentID,
                         TestResult = @TestResult,
                         Notes = @Notes,
                         CreatedByUserID = @CreatedByUserID
                     WHERE TestID = @TestID";

            using (SqlConnection connection =
                   new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.Add("@TestID", SqlDbType.Int).Value = TestDTO.TestID;
                command.Parameters.Add("@TestAppointmentID", SqlDbType.Int).Value = TestDTO.TestAppointmentID;
                command.Parameters.Add("@TestResult", SqlDbType.TinyInt).Value = TestDTO.TestResult;
                command.Parameters.Add("@CreatedByUserID", SqlDbType.Int).Value = TestDTO.CreatedByUserID;

                if (string.IsNullOrEmpty(TestDTO.Notes))
                {
                    command.Parameters.AddWithValue("@Notes", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@Notes", TestDTO.Notes);    
                }

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch
                {
                    return false;
                }
            }

            return (rowsAffected > 0);
        }


        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte count = 0;

            string query = @"SELECT COUNT(*) AS PassedTestCount
                             FROM TestAppointments TA
                             INNER JOIN Tests T
                                     ON T.TestAppointmentID = TA.TestAppointmentID
                             WHERE TA.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                               AND T.TestResult = 1";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            count = Convert.ToByte(result);
                        }
                    }
                    catch
                    {
                        count = 0;
                    }
                }

            }
            return count;
        }

    }
}
