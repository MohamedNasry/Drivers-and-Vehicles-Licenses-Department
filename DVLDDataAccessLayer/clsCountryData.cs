using DVLD_Loggin;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccessLayer
{
    public class clsCountryData
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;
            const string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", ID);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            CountryName = reader["CountryName"] as string ?? string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogError(ex, nameof(GetCountryInfoByID));
                isFound = false;
            }

            return isFound;
        }

        public static bool GetCountryInfoByName(ref int ID, string CountryName)
        {
            bool isFound = false;
            const string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            ID = reader["CountryID"] != DBNull.Value ? Convert.ToInt32(reader["CountryID"]) : -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogError(ex, nameof(GetCountryInfoByName));
                isFound = false;
            }

            return isFound;
        }

        public static DataTable GetAllCountries()
        {
            var dt = new DataTable();
            const string query = "SELECT * FROM Countries ORDER BY CountryName";

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
                EventLogger.LogError(ex, nameof(GetAllCountries));
            }

            return dt;
        }

        public static int AddNewCountry(string CountryName)
        {
            int CountryID = -1;
            const string query = @"INSERT INTO Countries (CountryName) 
                                   VALUES (@CountryName);
                                   SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);
                    connection.Open();

                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        CountryID = insertedID;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogError(ex, nameof(AddNewCountry));
            }

            return CountryID;
        }

        public static bool UpdateCountry(int CountryID, string CountryName)
        {
            int rowsAffected = 0;
            const string query = @"UPDATE Countries
                                   SET CountryName = @CountryName
                                   WHERE CountryID = @CountryID";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);
                    command.Parameters.AddWithValue("@CountryName", CountryName);
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogError(ex, nameof(UpdateCountry));
            }

            return rowsAffected > 0;
        }

        public static bool DeleteCountry(int CountryID)
        {
            int rowsAffected = 0;
            const string query = "DELETE FROM Countries WHERE CountryID = @CountryID";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", CountryID);
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogError(ex, nameof(DeleteCountry));
            }

            return rowsAffected > 0;
        }

        public static bool IsCountryExist(int ID)
        {
            bool isFound = false;
            const string query = "SELECT 1 FROM Countries WHERE CountryID = @CountryID";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", ID);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        isFound = reader.HasRows;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogError(ex, nameof(IsCountryExist));
                isFound = false;
            }

            return isFound;
        }

        public static bool IsCountryExist(string CountryName)
        {
            bool isFound = false;
            const string query = "SELECT 1 FROM Countries WHERE CountryName = @CountryName";

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.connectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        isFound = reader.HasRows;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogger.LogError(ex, nameof(IsCountryExist));
                isFound = false;
            }

            return isFound;
        }
    }
}