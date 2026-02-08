using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationData
    {

        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID,
           ref int ApplicationID,ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);


            string query = "Select * From LocalDrivingLicenseApplications Where LocalDrivingLicenseApplicationID = @LDLApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LDLApplicationID", LocalDrivingLicenseApplicationID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];

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
                }

                connection.Close();

            }

            return isFound;

        }


        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(int ApplicationID,ref int LocalDrivingLicenseApplicationID,
            ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);


            string query = "Select * From LocalDrivingLicenseApplications Where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];

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
                }

                connection.Close();

            }

            return isFound;
        }


        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection( clsDataAccessSettings.connectionString);


            string query = @"SELECT *
                              FROM LocalDrivingLicenseApplications_View
                              order by ApplicationDate Desc";


            SqlCommand command = new SqlCommand(query,connection);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if ( reader.HasRows)
                {
                    dt.Load(reader);
                    
                }
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

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID,int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into LocalDrivingLicenseApplications (ApplicationID,LicenseClassID)
                                                        Values
                                                                         (@ApplicationID,@LicenseClassID);
                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                {
                    LocalDrivingLicenseApplicationID = InsertedID;
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return LocalDrivingLicenseApplicationID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection (clsDataAccessSettings.connectionString);

            string query = @"Update LocalDrivingLicenseApplications Set
                                            ApplicationID = @ApplicationID,
                                            LicenseClassID = @LicenseClassID
                            Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand (query, connection);

            command.Parameters.AddWithValue("@ApplicationID",ApplicationID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Delete LocalDrivingLicenseApplications 
                                                    Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


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
            return (rowsAffected > 0);
        }
        public static bool IsLicenseClassIDExist(int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select Found = 1 From LocalDrivingLicenseApplications Where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                isFound = reader.HasRows;
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
                }
                connection.Close();
            }

            return isFound;

        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {

            bool Result = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @" SELECT top 1 Found=1
                            FROM
                                 TestAppointments 
                            WHERE
                            (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)  
                            AND(TestTypeID = @TestTypeID) and isLocked=0
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null)
                {
                    Result = true;
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

            return Result;

        }


        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Top 1 Found = 1
                            From LocalDrivingLicenseApplications inner join TestAppointments
                            On LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                            inner join Tests on TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                            and TestAppointments.TestTypeID = @TestTypeID
                            Order by TestAppointments.TestAppointmentID Desc;";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID",TestTypeID);


            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null)
                {
                    isFound = true;
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            byte TotalTrialsPerTest = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select TotalTrialsPerTest = Count(*) From
                          LocalDrivingLicenseApplications Inner join TestAppointments On LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                          Inner join Tests On TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                          Where (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplication)
                          And (TestAppointments.TestTypeID = @TestTypeID)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplication", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(),out byte Trials))
                {
                    TotalTrialsPerTest = Trials;
                }
            }
            catch (Exception ex)
            {
                TotalTrialsPerTest = 0;
            }
            finally
            {
                connection.Close();
            }

            return TotalTrialsPerTest;
        }




        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)

        {


            bool Result = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @" SELECT top 1 TestResult
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            And Tests.TestResult = 1
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    Result = Convert.ToBoolean(result);
                }

                //if (result != null && bool.TryParse(result.ToString(), out bool returnedResult))
                //{
                //    Result = returnedResult;
                //}
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }

            return Result;

        }


        //public bool PassedAllTests()
        //{
        //    return clsTest.PassedAllTests(this.LocalDrivingLicenseApplicationID);
        //}

        //public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        //{
        //    //if total passed test less than 3 it will return false otherwise will return true
        //    return clsTest.PassedAllTests(LocalDrivingLicenseApplicationID);
        //}

        //public bool IsLicenseIssued()
        //{
        //    // return (GetActiveLicenseID() != -1);
        //}

        //public int GetActiveLicenseID()
        //{//this will get the license id that belongs to this application
        //    return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        //}

    }
}