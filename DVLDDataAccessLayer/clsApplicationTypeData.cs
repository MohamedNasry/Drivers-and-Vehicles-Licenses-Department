using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsApplicationTypeData
    {
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, ref string ApplicationTypeTitle,
            ref Decimal ApplicationFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From ApplicationTypes where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationFees = Convert.ToDecimal(reader["ApplicationFees"]);
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


        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection( clsDataAccessSettings.connectionString);

            string query = "Select * From ApplicationTypes order by ApplicationTypeID";

            SqlCommand command = new SqlCommand(query,connection);

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
            catch(Exception ex)
            {

            }
            finally
            {
                if (reader!=null)
                {
                    reader.Close();
                }
                connection.Close();
            }

            return dt;



        }


        public static bool UpdateApplicationTypes(int ApplicationTypeID,string ApplicationTypeTitle,Decimal ApplicationFees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update ApplicationTypes 
                                                    Set ApplicationTypeTitle = @ApplicationTypeTitle,
                                                    ApplicationFees = @ApplicationFees
                                        Where ApplicationTypeID = @ApplicationTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeID",ApplicationTypeID);
            command.Parameters.AddWithValue("@ApplicationTypeTitle",ApplicationTypeTitle);
            command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch(Exception ex)
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
