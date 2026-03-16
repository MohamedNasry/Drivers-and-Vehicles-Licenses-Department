using DVLD_Loggin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsTestTypesData
    {
        public static bool GetTestTypeInfoByID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref decimal TestTypeFees)
        {
            bool isFound = false;
            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            TestTypeTitle = reader["TestTypeTitle"] != DBNull.Value ? (string)reader["TestTypeTitle"] : "";
                            TestTypeDescription = reader["TestTypeDescription"] != DBNull.Value ? (string)reader["TestTypeDescription"] : "";
                            TestTypeFees = reader["TestTypeFees"] != DBNull.Value ? Convert.ToDecimal(reader["TestTypeFees"]) : 0m;
                        }
                    }
                }
                catch (Exception ex)
                {
                    EventLogger.LogError(ex, nameof(GetTestTypeInfoByID));
                    isFound = false;
                }
            }

            return isFound;
        }

        public static DataTable GetAllTestType()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM TestTypes ORDER BY TestTypeID";

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
                    EventLogger.LogError(ex, nameof(GetAllTestType));
                    dt = null;
                }
            }

            return dt;
        }

        public static bool UpdateTestType(int ID, string Title, string Description, decimal Fees)
        {
            int rowsAffected = 0;
            string query = @"
                UPDATE TestTypes
                SET TestTypeTitle = @Title,
                    TestTypeDescription = @Description,
                    TestTypeFees = @Fees
                WHERE TestTypeID = @ID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
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
                    EventLogger.LogError(ex, nameof(UpdateTestType));
                    rowsAffected = 0;
                }
            }

            return rowsAffected > 0;
        }
    }
}