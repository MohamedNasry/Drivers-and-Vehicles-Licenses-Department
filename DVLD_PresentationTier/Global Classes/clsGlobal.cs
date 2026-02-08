using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD_PresentationTier
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPassword(string UserName, string Password)
        {
            try
            {
                //this will get the current project directory forder
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

                //Define the path to the text file where you want to save the data
                string filePath = CurrentDirectory + "\\data.txt";

                //incase the username is empty. delete the file
                if (UserName == "" && File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }

                //concatonate username and password with seperstor
                string dataToSave = UserName + "#//#" + Password;

                //Create a streamWrite to write to the file
                using(StreamWriter writer = new StreamWriter(filePath))
                {
                    //Write the data to the file
                    writer.WriteLine(dataToSave);
                    return true;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public static bool GetStoredGredential(ref string username, ref string password)
        {
            //this will get the stored username and password and will return true if found and false if not found.

            try
            {
                //gets the current project's directory
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

                //Define the path to the text file where you want to save the data
                string filePath = CurrentDirectory + "\\data.txt";


                // Check if the file exists before attempting to read it

                if (File.Exists(filePath))
                {
                    // Create a StreamReader to read from the file
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // Read data line by line until the end of the file
                        string Line;
                        while ((Line = reader.ReadLine()) != null)
                        {
                            //Console.WriteLine(Line);
                            string[] result = Line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            username = result[0];
                            password = result[1];

                        }
                        return true;
    
                    }

                }
                else
                    return false;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }
    }
}
