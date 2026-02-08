using DVLD_PresentationTier.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationTier.Tests.Controles
{
    public partial class ctrlScheduledTest : UserControl
    {
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentId = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private int _TestID = -1;   
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            gbTestType.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }
                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbTestType.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            gbTestType.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;
                        }
                }
            }
        }

        public int TestAppointmentID
        {
            get
            {
                return _TestAppointmentId;
            }
        }

        public int TestID
        {
            get
            {
                return _TestID;
            }
        }

        public ctrlScheduledTest()
        {
            InitializeComponent();
           
        }

        public void LoadInfo(int testAppointmentId)
        {
            _TestAppointmentId = testAppointmentId;

            _TestAppointment = clsTestAppointment.Find(_TestAppointmentId);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Test Appointment with ID = " + _TestAppointmentId,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _TestID = _TestAppointment.TestID;

            _LocalDrivingLicenseApplicationID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);


            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _TestAppointment.LocalDrivingLicenseApplicationID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalDrivingLicenseApplicationID.Text = _TestAppointment.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.FullName;
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();

            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblDate.Text = clsFormat.DateToShort(_TestAppointment.AppointmentDate);

            lblTestID.Text = (_TestID == -1) ? "Not Taken Yet" : _TestID.ToString();

        }


    }
}
