using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsApplicationData
    {

        public static bool GetApplicationInfoByID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, 
            ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate,ref float PaidFees,
            ref int CreateByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From Applications Where ApplicationID = @ApplicationID";

            SqlCommand commmand = new SqlCommand(query, connection);

            commmand.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = commmand.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = Convert.ToSingle( reader["PaidFees"]);
                    CreateByUserID = (int)reader["CreatedByUserID"];
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


        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From Applications Order By ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
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


        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            int ApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Applications 
                                        (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, [LastStatusDate], PaidFees, CreatedByUserID)
                                        Values 
                                        (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);

                            Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@ApplicantPersonID",ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate",LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees",PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID",CreatedByUserID);

            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationID = insertedID;
                }


            }
            catch (Exception ex)
            {
                ApplicationID = -1;
            }
            finally
            {
                connection.Close();
            }


            return ApplicationID;
        }


        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID,DateTime ApplicationDate,
            int ApplicationTypeID,byte ApplicationStatus,DateTime LastStatusDate,float PaidFees,int CreateByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Applications 
                                                Set ApplicantPersonID = @ApplicantPersonID,
                                                    ApplicationDate = @ApplicationDate,
                                                    ApplicationTypeID = @ApplicationTypeID,
                                                    ApplicationStatus = @ApplicationStatus,
                                                    LastStatusDate = @LastStatusDate,
                                                    PaidFees = @PaidFees,
                                                    CreateByUser = @CreateByUser
                            where ApplicationID = @ApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@CreateByUserID", CreateByUserID);

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


        public static bool DeleteApplication(int ApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Delete Applications Where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select Found = 1 From Applications Where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            SqlDataReader reader = null;
            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                isFound = reader.HasRows;

            }
            catch
            {
                isFound = false;
            }
            finally
            {
                reader.Close();
            }

            return isFound;

        }


        public static bool DoesPersonHaveActiveApplication (int PersonID, int ApplicationTypeID)
        {
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
        }

        public static int GetActiveApplicationID (int PersonID,int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select ActiveApplicationID = ApplicationID From Applications Where ApplicantPersonID = @ApplicationPersonID
                                                                and ApplicationTypeID = @ApplicationTypeID and ApplicationStatus = 1";

            SqlCommand command = new SqlCommand (query,connection);

            command.Parameters.AddWithValue("@ApplicationPersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int AppID))
                {
                    ActiveApplicationID = AppID;
                }
            }
            catch
            {
                ActiveApplicationID=-1;
            }
            finally
            {
                connection.Close();
            }

            return ActiveApplicationID;

        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID,int ApplicationTypeID,int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select ActiveApplicationID = Applications.ApplicationID
                            From Applications Inner Join 
                                        LocalDrivingLicenseApplications On Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            where ApplicantPersonID = @ApplicantPersonID
                            and ApplicationTypeID = @ApplicationTypeID
                            and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            and ApplicationStatus = 1;";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            command.Parameters.AddWithValue("@LicenseClassID",LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(),out int AppID))
                {
                    ActiveApplicationID = AppID;
                }
            }
            catch
            {
                ActiveApplicationID=-1;
            }
            finally
            {
                connection.Close();
            }

            return ActiveApplicationID;


        }


        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Applications Set 
                                                    ApplicationStatus = @NewStatus,
                                                    LastStatusDate = @LastStatusDate
                                            Where   ApplicationID = @ApplicationID;";

            SqlCommand command = new SqlCommand(@query,connection);

            command.Parameters.AddWithValue("@NewStatus",NewStatus);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();


            }
            catch
            {
                rowsAffected = 0;
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;


        }


    }
}
