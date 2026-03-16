using DVLD_Loggin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsApplicationTypeData
    {
   
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, ref string ApplicationTypeTitle,
            ref decimal ApplicationFees)
        {
            bool isFound = false;
            const string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            ApplicationTypeTitle = reader["ApplicationTypeTitle"] as string ?? string.Empty;
                            ApplicationFees = reader["ApplicationFees"] != DBNull.Value
                                ? Convert.ToDecimal(reader["ApplicationFees"])
                                : 0m;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // يمكن هنا تسجيل الخطأ في ملف لوج
                EventLogger.LogError(ex, nameof(GetApplicationTypeInfoByID));
                isFound = false;
            }

            return isFound;
        }
        public static DataTable GetAllApplicationTypes()
        {
            var dt = new DataTable();
            const string query = "SELECT * FROM ApplicationTypes ORDER BY ApplicationTypeID";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
          
                EventLogger.LogError(ex, nameof(GetAllApplicationTypes));
            }

            return dt;
        }

 
        public static bool UpdateApplicationTypes(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            int rowsAffected = 0;
            const string query = @"UPDATE ApplicationTypes
                                   SET ApplicationTypeTitle = @ApplicationTypeTitle,
                                       ApplicationFees = @ApplicationFees
                                   WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                    command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogError(ex, nameof(UpdateApplicationTypes));
                rowsAffected = 0;
            }

            return rowsAffected > 0;
        }
    }
}