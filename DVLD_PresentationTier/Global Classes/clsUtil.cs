using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace DVLD_PresentationTier
{


    public class clsUtil
    {

        public static string GenerateGUID()
        {
            Guid myGuid = Guid.NewGuid();
            return myGuid.ToString();
        }

        public static bool CreateForderIfDoesNotExist(string ForderPath)
        {
            //Check if forder Exist
            try
            {
                if (!Directory.Exists(ForderPath))
                {
                    //create Forder If Does not Exist
                    Directory.CreateDirectory(ForderPath);
                    return true;
                }
            }
            catch 
            {
                return false;
            }
            return true;
        }


        public static string ReplaceFileNameWithGuid(string SourceFile)
        {
            //Full file name : Change your file name
            string fileName = SourceFile;

            FileInfo fi = new FileInfo(fileName);

            string exth = fi.Extension;

            return GenerateGUID() + exth;
        }



        //public string DestinationPath = @"C:\DVLD-People-Images";
        public static bool CopyImageTpProjectImagesForder(ref string SourceFile)
        {
            //this function will copy the image to the 
            //project images folder after renaming it
            //with Guid with the same extention, then will update the sourceFileName with DestinationFileName


             string DestinationFolder = @"C:\DVLD-People-Images\";

            if (!CreateForderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            string DestinationFile = DestinationFolder + ReplaceFileNameWithGuid(SourceFile);

            try
            {
                File.Copy(SourceFile, DestinationFile, true);
            }
            catch(IOException iox)
            {
                MessageBox.Show( iox.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }

            SourceFile = DestinationFile;

            return true;

        }
    }
}
