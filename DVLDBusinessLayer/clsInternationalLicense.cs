using DVLD_Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccessLayer;
using System.Data;

namespace DVLDBusinessLayer
{
    public class clsInternationalLicense : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;


        private clsInternationalLicenseDTO _InternationalLicenseDTO;

        public int InternationalLicenseID
        {
            get { return _InternationalLicenseDTO.InternationalLicenseID; }
            set { _InternationalLicenseDTO.InternationalLicenseID = value; }
        }
        public int ApplicantID
        {
            get { return _InternationalLicenseDTO.ApplicantID; }
            set { _InternationalLicenseDTO.ApplicantID = value; }
        }
        public int DriverID
        {
            get { return _InternationalLicenseDTO.DriverID; }
            set { _InternationalLicenseDTO.DriverID = value; }
        }

        public clsDrivers DriverInfo;
        public int IssuedUsingLocalLicenseID
        {
            get { return _InternationalLicenseDTO.IssuedUsingLocalLicenseID; }
            set { _InternationalLicenseDTO.IssuedUsingLocalLicenseID = value; }
        }
        public DateTime IssueDate
        {
            get { return _InternationalLicenseDTO.IssueDate; }
            set { _InternationalLicenseDTO.IssueDate = value; }
        }
        public DateTime ExpirationDate
        {
            get { return _InternationalLicenseDTO.ExpirationDate; }
            set { _InternationalLicenseDTO.ExpirationDate = value; }
        }
        public bool IsActive
        {
            get { return _InternationalLicenseDTO.IsActive; }
            set { _InternationalLicenseDTO.IsActive = value; }
        }
        public int CreatedByUserID
        {
            get { return _InternationalLicenseDTO.CreatedByUserID; }
            set {
                base.CreatedByUserID = value;
                _InternationalLicenseDTO.CreatedByUserID = value; }
        }

        public clsInternationalLicense()
        {
            this.ApplicationTypeID =  (int)clsApplication.enApplicationType.NewInternationalLicense;
            _InternationalLicenseDTO = new clsInternationalLicenseDTO();
            _Mode = enMode.AddNew;
        }

        private clsInternationalLicense(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, enApplicationStatus ApplicationStatus,
            int ApplicationTypeID, DateTime LastStatusDate, float PaidFees, int CreatedByUserID, clsInternationalLicenseDTO dto) :
            base(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationStatus, ApplicationTypeID, LastStatusDate, PaidFees, CreatedByUserID)
        {
            

            _InternationalLicenseDTO = new clsInternationalLicenseDTO
            {
                InternationalLicenseID = dto.InternationalLicenseID,
                ApplicantID = ApplicationID,
                DriverID = dto.DriverID,
                IssuedUsingLocalLicenseID = dto.IssuedUsingLocalLicenseID,
                IssueDate = dto.IssueDate,
                ExpirationDate = dto.ExpirationDate,
                IsActive = dto.IsActive,
                CreatedByUserID = dto.CreatedByUserID
            };

            DriverInfo = clsDrivers.FindByDriverID(dto.DriverID);

            _Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {
            _InternationalLicenseDTO.ApplicantID = this.ApplicationID;
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(_InternationalLicenseDTO);

            return this.InternationalLicenseID != -1;

        }

        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(_InternationalLicenseDTO);

        }


        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            clsInternationalLicenseDTO dto = new clsInternationalLicenseDTO();
            dto.InternationalLicenseID = InternationalLicenseID;

            if (clsInternationalLicenseData.GetInternationalLicenseByID(dto))
            {
                clsApplication Application = clsApplication.FindBaseApplication(dto.ApplicantID);
                return new clsInternationalLicense(Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate, Application.ApplicationStatus,
                    (int)clsApplication.enApplicationType.NewInternationalLicense, Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID, dto);
            }
            else
            {
                return null;

            }

        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        public bool Save()
        {
            base.Mode = (clsApplication.enMode)_Mode;

            if (!base.Save())
            {
                return false;
            }

            switch(_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateInternationalLicense();
               
            }

            return false;

        }


        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(DriverID);
        }

        public static DataTable GetInternationalLicensesByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }


    }
}
