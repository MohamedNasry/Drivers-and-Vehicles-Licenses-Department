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
    public class clsDrivers
    {
        public enum enMode { AddNew = 0, Update = 1 }

        private enMode _Mode = enMode.AddNew;

        private clsDriverDTO _DriverDTO;

        public clsPerson PersonInfo;

        public int DriverId
        {
            get => _DriverDTO.DriverId;
            set => _DriverDTO.DriverId = value;
        }
        public int PersonId
        {
            get => _DriverDTO.PersonId;
            set => _DriverDTO.PersonId = value;
        }
        public int CreatedByUserID
        {
            get => _DriverDTO.CreatedByUserID;
            set => _DriverDTO.CreatedByUserID = value;
        }
        public DateTime CreatedDate
        {
            get => _DriverDTO.CreatedDate;
            set => _DriverDTO.CreatedDate = value;
        }

        public clsDrivers()
        {
            _DriverDTO = new clsDriverDTO();
            _Mode = enMode.AddNew;
        }

        private clsDrivers(clsDriverDTO DTO)
        {
            _DriverDTO = new clsDriverDTO
            {
                DriverId = DTO.DriverId,
                PersonId = DTO.PersonId,
                CreatedByUserID = DTO.CreatedByUserID,
                CreatedDate = DTO.CreatedDate
            };

            PersonInfo = clsPerson.Find(this.PersonId);

            _Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            this.DriverId = clsDriversData.AddNewDriver(_DriverDTO);

            return this.DriverId > -1;
        }

        private bool _UpdateDriver()
        {
            return clsDriversData.UpdateDriver(this.DriverId, _DriverDTO);
        }


        public static clsDrivers FindByDriverID(int DriverID)
        {
            clsDriverDTO dto = new clsDriverDTO();

            bool isFound = clsDriversData.GetDriverInfoByID(DriverID, dto);

            if (isFound)
            {
                return new clsDrivers(dto);
            }
            else
            {
                return null;
            }
        }

        public static clsDrivers FindByPersonID(int PersonID)
        {
            clsDriverDTO dto = new clsDriverDTO();
            bool isFound = clsDriversData.GetDriverInfoByPersonID(PersonID, dto);
            if (isFound)
            {
                return new clsDrivers(dto);
            }
            else
            {
                return null;
            }
        }


        public bool Save()
        {
            
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateDriver();
            }
            
            return false;
        }

        public static DataTable GetAll()
        {
            return clsDriversData.GetAll();
        }

        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }

        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetInternationalLicensesByDriverID(DriverID);
        }
    }
}
