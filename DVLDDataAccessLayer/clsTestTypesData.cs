using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsTestTypesData
    {
        public static bool GetTestTypeInfoByID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription,
            ref Decimal TestTypeFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From TestTypes where TestTypeID = @TestTypeID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID",TestTypeID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    TestTypeDescription = (string)reader["TestTypeDescription"];
                    TestTypeFees = (Decimal)reader["TestTypeFees"];
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


        public static DataTable GetAllTestType()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From TestTypes Order By TestTypeID";

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


        public static bool UpdateTestType(int ID,string Title,string Description,Decimal Fees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update TestTypes Set
                                                  TestTypeTitle = @Title,
                                                  TestTypeDescription = @Description,
                                                  TestTypeFees = @Fees
                             Where TestTypeID = @ID";

            SqlCommand command = new SqlCommand(query,connection);

            command.Parameters.AddWithValue("@ID", ID);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@Fees", Fees);

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

    }
}
