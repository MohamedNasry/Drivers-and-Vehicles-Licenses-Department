using DVLD_Domain.DTO;
using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsDetainedLicense
    {

        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode = enMode.AddNew;

        private clsDetainedDTO _DetainedLicense;

        public int DetainID
        {
            get => _DetainedLicense.DetainID;
            set => _DetainedLicense.DetainID = value;
        }
        public int LicenseID
        {
            get => _DetainedLicense.LicenseID;
            set => _DetainedLicense.LicenseID = value;
        }
        public DateTime DetainDate
        {
            get => _DetainedLicense.DetainDate;
            set => _DetainedLicense.DetainDate = value;
        }
        public double FineFees
        {
            get => _DetainedLicense.FineFees;
            set => _DetainedLicense.FineFees = value;
        }
        public int CreatedByUserID
        {
            get => _DetainedLicense.CreatedByUserID;
            set => _DetainedLicense.CreatedByUserID = value;
        }

        public clsUser CreatedByUserInfo;
        public bool IsReleased
        {
            get => _DetainedLicense.IsReleased;
            set => _DetainedLicense.IsReleased = value;
        }
        public DateTime ReleaseDate
        {
            get => _DetainedLicense.ReleaseDate;
            set => _DetainedLicense.ReleaseDate = value;
        }
        public int ReleasedByUserID
        {
            get => _DetainedLicense.ReleasedByUserID;
            set => _DetainedLicense.ReleasedByUserID = value;
        }

        public clsUser ReleasedByUserInfo;
        public int ReleaseApplicationID
        {
            get => _DetainedLicense.ReleaseApplicationID;
            set => _DetainedLicense.ReleaseApplicationID = value;
        }

        public clsDetainedLicense()
        {
            _DetainedLicense = new clsDetainedDTO();
            Mode = enMode.AddNew;

        }

        private clsDetainedLicense(clsDetainedDTO DetainedLicense)
        {
            _DetainedLicense = new clsDetainedDTO
            {
                DetainID = DetainedLicense.DetainID,
                LicenseID = DetainedLicense.LicenseID,
                DetainDate = DetainedLicense.DetainDate,
                FineFees = DetainedLicense.FineFees,
                CreatedByUserID = DetainedLicense.CreatedByUserID,
                IsReleased = DetainedLicense.IsReleased,
                ReleaseDate = DetainedLicense.ReleaseDate,
                ReleasedByUserID = DetainedLicense.ReleasedByUserID,
                ReleaseApplicationID = DetainedLicense.ReleaseApplicationID
            };

            CreatedByUserInfo = clsUser.FindByUserID(_DetainedLicense.CreatedByUserID);
            ReleasedByUserInfo = clsUser.FindByUserID(_DetainedLicense.ReleasedByUserID);

            Mode = enMode.Update;
        }


        private bool _AddNewDetainedLicense()
        {
            _DetainedLicense.DetainID = clsDetainedLicensesData.AddNewDetainedLicense(_DetainedLicense);
            return _DetainedLicense.DetainID > 0;
        }

        private bool _UpdateDetainedLicense()
        {
            return clsDetainedLicensesData.UpdateDetainedLicense(_DetainedLicense);
        }

        public static clsDetainedLicense FindByDetainID(int DetainID)
        {
            clsDetainedDTO detainedDTO = new clsDetainedDTO();
            detainedDTO.DetainID = DetainID;

            if (clsDetainedLicensesData.GetDetainedLicenseInfoByID(detainedDTO))
            {
                return new clsDetainedLicense(detainedDTO);
            }
            else
            {
                return null;
            }
        }

        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            clsDetainedDTO detainedDTO = new clsDetainedDTO();
            detainedDTO.LicenseID = LicenseID;
            if (clsDetainedLicensesData.GetDetainedLicenseInfoByLicenseID(detainedDTO))
            {
                return new clsDetainedLicense(detainedDTO);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllDetainedLicenses()
        {

            return clsDetainedLicensesData.GetAllDetainedLicenses();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateDetainedLicense();
            }
            return false;
        }

        public static bool isLicenseDetained(int LicenseID)
        {
            return clsDetainedLicensesData.isLicenseDetained(LicenseID);

        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            
           
            _DetainedLicense.ReleasedByUserID = ReleasedByUserID;
            _DetainedLicense.ReleaseApplicationID = ReleaseApplicationID;
           
            return clsDetainedLicensesData.ReleaseDetainedLicense(_DetainedLicense);

        }
    }
}