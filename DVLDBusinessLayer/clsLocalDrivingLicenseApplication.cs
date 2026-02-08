using DVLD_Domain.DTO;
using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        public int LocalDrivingLicenseApplicationID {  get; set; }
        //public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        public clsLicenseClass LicenseClassInfo;

        public string FullName
        {
            get
            { 
                return base.PersonInfo.FullName;

               // return clsPerson.Find(this.ApplicantPersonID).FullName;
            }
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID,
                this.ApplicationID,this.LicenseClassID);
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(this.ApplicationID,
                this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }
        
        public clsLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = -1;
         
            LicenseClassID = -1;

            Mode = enMode.AddNew;
        }

        //private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
        //    DateTime ApplicationDate, int ApplicationTypeID,
        //    enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees
        //    , int CreatedByUserID,int LicenseClassID)
        //{
        //    this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        //    this.ApplicationID = ApplicationID;
        //    this.ApplicantPersonID = ApplicantPersonID;
        //    this.ApplicationDate = ApplicationDate;
        //    this.ApplicationTypeID = ApplicationTypeID;
        //    this.ApplicationStatus = ApplicationStatus;
        //    this.LastStatusDate = LastStatusDate;
        //    this.PaidFees = PaidFees;
        //    this.CreatedByUserID = CreatedByUserID;
        //    this.LicenseClassID = LicenseClassID;
           

        //    LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);

        //   // ApplicationInfo = clsApplication.FindBaseApplication(ApplicationID);

        //    Mode = enMode.Update;
        //}

        private clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int ApplicantPersonID,
                    DateTime ApplicationDate, int ApplicationTypeID,
                    enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees,
                    int CreatedByUserID, int LicenseClassID)
                    : base(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationStatus, ApplicationTypeID, LastStatusDate, PaidFees, CreatedByUserID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.LicenseClassID = LicenseClassID;
            LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            Mode = enMode.Update;
        }


        public static clsLocalDrivingLicenseApplication FindByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID,
                ref ApplicationID,ref LicenseClassID))
            {

                //New we find the base application

                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                //we return new object of that Person the rigth data
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, Application.ApplicantPersonID
                    ,Application.ApplicationDate, Application.ApplicationTypeID, Application.ApplicationStatus, Application.LastStatusDate,
                    Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }
            else
            {
                return null;
            }
        }

        static public clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID(ApplicationID,
                ref LocalDrivingLicenseApplicationID,ref LicenseClassID))
            {

                //New we find the base application

                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                //we return new object of that Person the rigth data
                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, ApplicationID, Application.ApplicantPersonID
                    , Application.ApplicationDate, Application.ApplicationTypeID, Application.ApplicationStatus, Application.LastStatusDate,
                    Application.PaidFees, Application.CreatedByUserID, LicenseClassID);
            }
            else
            {
                return null;
            }
        }

        static public DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }


        static public bool isthereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, TestTypeID);
        }

        public bool Save()
        {
            //Because of inheretance first we call the save method in the base class
            //it will take care of adding all information to application table.

            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
            {
                return false;
            }


            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
            }

            return false;
        }


        public bool DoesPassTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }


        public bool Delete ()
        {
            bool IsLocalDrivingLicenseApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;



            IsLocalDrivingLicenseApplicationDeleted =  clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);

            if (!IsLocalDrivingLicenseApplicationDeleted)
                return false;

            IsBaseApplicationDeleted = base.Delete();

            return IsBaseApplicationDeleted;


        }

        public bool DoesAttendTestType (clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID,(int)TestTypeID);
        }

        public byte TotalTrialsPerTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);    
        }

    
 

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)

        {

            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType TestTypeID)

        {

            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }


        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public int IssueLicenseForTheFirstTime(string Notes, int CreatedByUserID)
        {

            //حماية الـ Business Layer أهم بكثير، 
            // لان يقدر ينادي الدالة من مكان اخر بدون ما يكون مراجع كل الشروط
            if (!isPassedAllTests())
                return -1;

            if (GetActiveLicenseID() != -1)
            {
                return -1;
            }


            int DriverID = -1;
            clsDrivers Driver = clsDrivers.FindByPersonID(this.ApplicantPersonID);

            if (Driver == null)
            {
                Driver = new clsDrivers();
                Driver.PersonId = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (!Driver.Save())
                {
                    return -1;
                }
                DriverID = Driver.DriverId;

            }
            else
            {
                DriverID = Driver.DriverId;
            }

            clsLicense License = new clsLicense();

            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClass = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = License.IssueDate.AddYears(this.LicenseClassInfo.DefaultValidityLength); 
            License.Notes = Notes;
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;

            if (License.Save())
            {
                this.SetComplete();

                return License.LicenseID;
            }
            else
            {
                return -1;
            }
        }

        public bool isPassedAllTests()
        {
            //return (GetPassedTestCount() == 3);

            return clsTest.PassedAllTests(this.LocalDrivingLicenseApplicationID);

        }

        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }

        public bool isLicenseIssued()
        {
            return clsLicense.IsLicenseExistByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }

    }

}
