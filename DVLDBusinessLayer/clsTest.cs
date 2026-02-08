using DVLD_Domain.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        private clsTestDTO _TestDTO;

        public clsTestAppointment TestAppointmentInfo;

        public int TestID
        {
                       get => _TestDTO.TestID;
            set => _TestDTO.TestID = value;

        }
        public int TestAppointmentID
        {
            get => _TestDTO.TestAppointmentID;
            set => _TestDTO.TestAppointmentID = value;
        }
        public byte TestResult
        {
            get => _TestDTO.TestResult;
            set => _TestDTO.TestResult = value;
        }
        public string Notes
        {
            get => _TestDTO.Notes;
            set => _TestDTO.Notes = value;
        }
        public int CreatedByUserID
        {
            get => _TestDTO.CreatedByUserID;
            set => _TestDTO.CreatedByUserID = value;
        }
        public clsTest()
        {
            _TestDTO = new clsTestDTO();
            Mode = enMode.AddNew;
        }

        private clsTest(clsTestDTO DTO)
        {
            _TestDTO = new clsTestDTO
            {
                TestID = DTO.TestID,
                TestAppointmentID = DTO.TestAppointmentID,
                TestResult = DTO.TestResult,
                Notes = DTO.Notes,
                CreatedByUserID = DTO.CreatedByUserID
            };

            TestAppointmentInfo = clsTestAppointment.Find(this.TestAppointmentID);

            Mode = enMode.Update;

        }

        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(this._TestDTO);
            return (this._TestDTO.TestID != -1);
        }

        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this._TestDTO);
        }



        public static clsTest Find(int TestID)
        {
            clsTestDTO DTO = new clsTestDTO();
            bool isFound = clsTestData.GetTestInfoByID(TestID, DTO);
            if (isFound)
            {
                return new clsTest(DTO);
            }
            else
            {
                return null;
            }
        }

        public static clsTest FindLastTestPerPersonAndLicenseClass(int PersonID, int LicenseClassID, clsTestType.enTestType TestType)
        {
            clsTestDTO DTO = new clsTestDTO();
            bool isFound = clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass(PersonID,(int) TestType,LicenseClassID,DTO);
            if (isFound)
            {
                return new clsTest(DTO);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAll()
        {
             
            return clsTestData.GetAllTests();

        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (!_AddNewTest())
                    {
                        return false;
                    }
                    else
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                case enMode.Update:
                    return _UpdateTest();

            }
            return false;
        }


        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }


        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }
    }
}
