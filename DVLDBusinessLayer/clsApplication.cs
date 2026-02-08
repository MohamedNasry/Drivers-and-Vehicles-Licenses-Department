using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enum enApplicationType { NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicense = 5, NewInternationalLicense = 6, RetakeTest = 8 };

        public enMode Mode;

        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 };

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }

        public clsPerson PersonInfo { get; set; }
        public string ApplicantFullName
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }

        public enApplicationStatus ApplicationStatus { get; set; }
        public string StatusTest
        {
            get 
            {
                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }

            }
        }
        public int CreatedByUserID { get; set; }

        public clsApplicationType ApplicationTypeInfo;

        public clsUser CreatedByUserInfo;

        public clsApplication()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationStatus = enApplicationStatus.New;
            ApplicationTypeID = -1;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }
        
        public clsApplication(int ApplicationID,int ApplicantPersonID,DateTime ApplicationDate,enApplicationStatus ApplicationStatus,
            int ApplicationTypeID , DateTime LastStatusDate,float PaidFees, int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;

            PersonInfo = clsPerson.Find(ApplicantPersonID);

            this.ApplicationDate = ApplicationDate;
            this.ApplicationStatus = ApplicationStatus;
            this.ApplicationTypeID = ApplicationTypeID;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;

            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
          
            CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);

            Mode = enMode.Update;

        }


        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(ApplicantPersonID, ApplicationDate,ApplicationTypeID,
                (byte)ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID);

            return this.ApplicationID != -1;
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(ApplicationID,ApplicantPersonID,ApplicationDate,ApplicationTypeID,
                (byte)ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID);
        }

        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID = -1, ApplicationTypeID = -1, CreatedByUserID = -1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            byte ApplicationStatus = 1;
            float PaidFees = 0; 


            if (clsApplicationData.GetApplicationInfoByID(ApplicationID,ref ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID,
                ref ApplicationStatus,ref LastStatusDate,ref PaidFees,ref CreatedByUserID))
            {
                return new clsApplication(ApplicationID,ApplicantPersonID,ApplicationDate,(enApplicationStatus)ApplicationStatus,ApplicationTypeID,
                    LastStatusDate,PaidFees,CreatedByUserID);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllApplications()
        {
            return clsApplicationData.GetAllApplications();
        }


        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        { return false; }
                case enMode.Update:
                    if (_UpdateApplication())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            return false;
        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, 2);
        }

        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(this.ApplicationID, 3);
        }

        public bool Delete()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID);
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }

        public bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        }

        //public  int GetActiveApplicationID()
        //{
        //    return clsApplicationData.GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        //}
        public static int GetActiveApplicationID(int PersonID, enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public int GetActiveApplicationID(enApplicationType ApplicationTypeID)
        {
            return GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        }





    }
}
