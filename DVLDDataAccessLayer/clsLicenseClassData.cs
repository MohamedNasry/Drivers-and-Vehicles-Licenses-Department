using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    public class clsLicenseClassData
    {
        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge,
            ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From LicenseClasses Where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID",LicenseClassID);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ClassName = (string)reader["ClassName"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle(reader["ClassFees"]);
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

        public static bool GetLicenseClassInfoByName(string ClassName, ref int LicenseClassID, ref string ClassDescription, ref byte MinimumAllowedAge,
            ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From LicenseClasses Where ClassName = @ClassName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);

            SqlDataReader reader = null;

            try
            {
                connection.Open();

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    LicenseClassID = (int)reader["LicenseClassID"];
                    ClassDescription = (string)reader["ClassDescription"];
                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                    ClassFees = Convert.ToSingle (reader["ClassFees"]);
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

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From LicenseClasses";

            SqlCommand command = new SqlCommand (query, connection);

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


        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
         byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into LicenseClasses 
           (
            ClassName,ClassDescription,MinimumAllowedAge, 
            DefaultValidityLength,ClassFees)
                            Values ( 
            @ClassName,@ClassDescription,@MinimumAllowedAge, 
            @DefaultValidityLength,@ClassFees)
                            
                            SELECT SCOPE_IDENTITY();";



            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);
            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", ClassFees);



            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseClassID = insertedID;
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


            return LicenseClassID;

        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowedAge,
            byte DefaultValidityLength, float ClassFees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update LicenseClasses Set 
                                                        ClassName = @ClassName,
                                                        ClassDescription = @ClassDescription,
                                                        MinimumAllowedAge = @MinimumAllowedAge,
                                                        DefaultValidityLength = @DefaultValidityLength
                                                        ClassFees = @ClassFees
                            Where LicenseClassID = @LicenseClassID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID",LicenseClassID);
            command.Parameters.AddWithValue("@ClassName", ClassName);
            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
            command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
            command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
            command.Parameters.AddWithValue("@ClassFees", ClassFees);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);


        }



    }
}
