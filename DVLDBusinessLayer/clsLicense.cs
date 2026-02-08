using DVLD_Domain.DTO;
using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Domain.DTO.clsLicenseDTO;

namespace DVLDBusinessLayer
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode = enMode.AddNew;

        private clsLicenseDTO _LicenseDTO;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };


        public int LicenseID
        {
            get => _LicenseDTO.LicenseID;
            set => _LicenseDTO.LicenseID = value;
        }
        public int ApplicationID
        {
            get => _LicenseDTO.ApplicationID;
            set => _LicenseDTO.ApplicationID = value;
        }
        public int DriverID
        {
            get => _LicenseDTO.DriverID;
            set => _LicenseDTO.DriverID = value;
        }

        public clsDrivers DriverInfo;
        public int LicenseClass
        {
            get => _LicenseDTO.LicenseClass;
            set => _LicenseDTO.LicenseClass = value;
        }

        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate
        {
            get => _LicenseDTO.IssueDate;
            set => _LicenseDTO.IssueDate = value;
        }
        public DateTime ExpirationDate
        {
            get => _LicenseDTO.ExpirationDate;
            set => _LicenseDTO.ExpirationDate = value;
        }
        public string Notes
        {
            get => _LicenseDTO.Notes;
            set => _LicenseDTO.Notes = value;
        }
        public double PaidFees
        {
            get => _LicenseDTO.PaidFees;
            set => _LicenseDTO.PaidFees = value;
        }
        public bool IsActive
        {
            get => _LicenseDTO.IsActive;
            set => _LicenseDTO.IsActive = value;
        }

        public enIssueReason IssueReason
        {
            get => (enIssueReason)_LicenseDTO.IssueReason;
            set => _LicenseDTO.IssueReason = (int)value;
        }

        public string IssueReasonText
        {
            get => GetIssueReasonText(this.IssueReason);
        }

        public bool IsDetained
        {
            get { return clsDetainedLicense.isLicenseDetained(this.LicenseID); }
        }

        public int CreatedByUserID
        {
            get => _LicenseDTO.CreatedByUserID;
            set => _LicenseDTO.CreatedByUserID = value;
        }

        public clsDetainedLicense DetainedLicenseInfo { get; set; }
        public clsLicense()
        {
            _LicenseDTO = new clsLicenseDTO();
            _Mode = enMode.AddNew;
        }


        private clsLicense(clsLicenseDTO DTO)
        {
            _LicenseDTO = new clsLicenseDTO
            {
                LicenseID = DTO.LicenseID,
                ApplicationID = DTO.ApplicationID,
                DriverID = DTO.DriverID,
                LicenseClass = DTO.LicenseClass,
                IssueDate = DTO.IssueDate,
                ExpirationDate = DTO.ExpirationDate,
                Notes = DTO.Notes,
                PaidFees = DTO.PaidFees,
                IsActive = DTO.IsActive,
                IssueReason = DTO.IssueReason,
                CreatedByUserID = DTO.CreatedByUserID
            };

            DriverInfo = clsDrivers.FindByDriverID(this.DriverID);
            LicenseClassInfo = clsLicenseClass.Find(this.LicenseClass);
            DetainedLicenseInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);
            _Mode = enMode.Update;
        }

        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(_LicenseDTO);
            return (this.LicenseID != -1);

        }

        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(_LicenseDTO);
        }

        public static clsLicense Find(int LicenseID)
        {
            clsLicenseDTO LicenseDTO = new clsLicenseDTO();
            LicenseDTO.LicenseID = LicenseID;
            bool isFound = clsLicenseData.GetLicenseInfoByLicenseID(LicenseDTO);

            if (isFound)
            {
                return new clsLicense(LicenseDTO);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }   

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateLicense();
            }
            return false;
        }
        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {

            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);

        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public bool IsLicenseExpired()
        {
            return DateTime.Now > this.ExpirationDate;

        }

        public bool DeactivateCurrentLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        public int Detain(double FineFees, int CreatedByUserID)
        {
            clsDetainedLicense DetainedLicense = new clsDetainedLicense();
            DetainedLicense.LicenseID = this.LicenseID;
            DetainedLicense.DetainDate = DateTime.Now;
            DetainedLicense.FineFees = FineFees;
            DetainedLicense.CreatedByUserID = CreatedByUserID;
            if (DetainedLicense.Save())
            {
                
                return DetainedLicense.DetainID;
            }
            else
            {
                return -1;
            }
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID,ref int ApplicationID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonId;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = (float)(clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense).ApplicationFees);
            Application.CreatedByUserID = ReleasedByUserID;

            if(!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = Application.ApplicationID;

            return this.DetainedLicenseInfo.ReleaseDetainedLicense(ReleasedByUserID, ApplicationID);
        }

        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {  //تجديد الرخصة
           // First create Application
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonId;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = (float)(clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees);
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;

            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (NewLicense.Save())
            {
                // Deactivate old license
                this.DeactivateCurrentLicense();
                return NewLicense;
            }
            else
            {
                return null;
            }

        }

        public clsLicense Replace(enIssueReason IssueReason, int CreatedByUserID)
        {
            //First create Application
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonId;
            Application.ApplicationDate = DateTime.Now;

            Application.ApplicationTypeID = (int)(IssueReason == enIssueReason.DamagedReplacement ? 
                clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
                clsApplication.enApplicationType.ReplaceLostDrivingLicense);

            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = (float)(clsApplicationType.Find(Application.ApplicationTypeID).ApplicationFees);
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0;// no fees for the license because it's a replacement.
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (NewLicense.Save())
            {
                // Deactivate old license
                this.DeactivateCurrentLicense();
                return NewLicense;
            }
            else
            {
                return null;
            }



        }


    }
}
