using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsTestAppointmentData
    {

        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From TestAppointments Where TestAppointmentID = @TestAppointmentID";

            SqlCommand Command = new SqlCommand(query, connection); 

            Command.Parameters.AddWithValue("@TestAppointmentID",TestAppointmentID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = Command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestTypeID = (int)reader["TestTypeID"];
                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = Convert.ToBoolean(reader["IsLocked"]);

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                    {
                        RetakeTestApplicationID = -1;
                    }
                    else
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
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


        public static bool GetLastTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID, ref int TestAppointmentID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Top 1 * From TestAppointments
                             Where   TestTypeID = @TestTypeID
                             And  LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             order by TestAppointmentID Desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    AppointmentDate = (DateTime)reader["AppointmentDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    IsLocked = (bool)reader["IsLocked"];
                    RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
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


        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From TestAppointments_View
                             Order By AppointmentDate Desc;";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;

        }


        public static DataTable GetApplicationTestAppointmentPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select TestAppointmentID ,AppointmentDate, PaidFees, IsLocked
                             From TestAppointments
                                Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                And TestTypeID = @TestTypeID
                            Order by AppointmentDate Desc;";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID",LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID",TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();

            }
            return dt;

        }


        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            float PaidFees, int CreatedByUserID, int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into TestAppointments(TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,
                                                          CreatedByUserID,IsLocked,RetakeTestApplicationID)
                            Values
                                    (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees
                                      ,@CreatedByUserID,0,@RetakeTestApplicationID)
                            Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees",PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            
            if (RetakeTestApplicationID == -1)

                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(),out int InsertedID))
                {
                    TestAppointmentID = InsertedID;
                }


            }
            catch (Exception ex)
            {
                TestAppointmentID = -1;
            }
            finally
            {
                connection.Close();
            }

            return TestAppointmentID;




        }


        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int RowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update TestAppointments Set TestTypeID = @TestTypeID,
                                                         LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                                         AppointmentDate = @AppointmentDate,
                                                         PaidFees = @PaidFees,
                                                         CreatedByUserID = @CreatedByUserID,
                                                         IsLocked = @IsLocked,
                                                         RetakeTestApplicationID = @RetakeTestApplicationID
                             Where TestAppointmentID = @TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection); 

            command.Parameters.AddWithValue("@TestAppointmentID",TestAppointmentID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@AppointmentDate",AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@IsLocked",IsLocked);
            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);


            try
            {
                connection.Open();

                RowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                RowsAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return RowsAffected > 0;

        }


        public static int GetTestID (int TestAppointmentID)
        {
            int TestID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select TestID From Tests 
                                Where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand (query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    TestID = ID;
                }

            }
            catch (Exception ex)
            {
                TestID = -1;
            }
            finally
            {
                connection.Close();
            }

            return TestID;

        }





    }
}
