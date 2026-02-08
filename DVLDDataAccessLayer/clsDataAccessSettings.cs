using System;
using System.Collections.Generic;


namespace DVLDDataAccessLayer
{
    static class clsDataAccessSettings
    {
        public static string connectionString
        {
            get
            {
                string server = Environment.GetEnvironmentVariable("DB_SERVER");
                string database = Environment.GetEnvironmentVariable("DB_NAME");
                string user = Environment.GetEnvironmentVariable("DB_USER");
                string password = Environment.GetEnvironmentVariable("DB_PASSWORD");

                var missing = new List<string>();
                if (string.IsNullOrWhiteSpace(server)) missing.Add("DB_SERVER");
                if (string.IsNullOrWhiteSpace(database)) missing.Add("DB_NAME");
                if (string.IsNullOrWhiteSpace(user)) missing.Add("DB_USER");
                if (string.IsNullOrWhiteSpace(password)) missing.Add("DB_PASSWORD");
                if (missing.Count > 0)
                    throw new InvalidOperationException($"Missing environment variables: {string.Join(", ", missing)}");

                return $"Server={server};Database={database};User Id={user};Password={password};";
            }
        }


      
            
    }
}
