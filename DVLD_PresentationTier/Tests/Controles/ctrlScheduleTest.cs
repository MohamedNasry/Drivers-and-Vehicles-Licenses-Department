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

namespace DVLD_PresentationTier
{
    public partial class ctrlScheduleTest : UserControl
    {

        public enum enMode { AddNew = 0, Update = 1}
        private enMode _Mode = enMode.AddNew;

        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 }
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private int _LocalDrivingLicenseApplicationID = -1;

        private clsTestAppointment _TestAppointment;

        private int _TestAppointmentID = -1;
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


        public ctrlScheduleTest()
        {
            InitializeComponent();
        }



        public void LoadInfo(int LocalDrivingLicenseApplicationID, int AppointmentID = -1)
        {
            //info apoointment id this means AddNew mode otherwise it's update mode
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppointmentID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null )
            {
                MessageBox.Show("Error: No Local driving license Application with ID = " + LocalDrivingLicenseApplicationID,
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);    
                btnSave.Enabled = false;
                return;
            }

            //decide if the creation mode is retake test or not based if the person attend any test
            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblRAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRTestAppID.Text = "0";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRAppFees.Text = "0";
                lblRTestAppID.Text = "N/A";

            }

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.FullName;

            //this will show the trials for this test before
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();

            if (_Mode == enMode.AddNew)
            {
                lblFees.Text = clsTestType.Find(_TestTypeID).TestTypeFees.ToString();
                dtpTestDate.MinDate = DateTime.Now;
                lblRTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRAppFees.Text)).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
            {
                return;
            }

            if (!_HandleAppointmentLockedConstraint())
            {
                return;
            }

            if (!_HandlePreviousTestConstraint())
            {
                return;
            } 


        }


        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = "+ _TestAppointmentID,
                    "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);    
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment.PaidFees.ToString();

            //we compare the current date with the appointment date to set the min date
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate;

            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRAppFees.Text = "0";
                lblRTestAppID.Text = "N/A";

            }
            else
            {
                lblRAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
            }
            return true;
        }


        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalDrivingLicenseApplication.isthereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID,(int)_TestTypeID))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = true;
                dtpTestDate.Enabled = true;
                return false;
            }
            return true;
        }

        private bool _HandleAppointmentLockedConstraint()
        {
            //if appointment is locked that means the person already sat for the test
            if (_TestAppointment.IsLocked)
            { 
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
                lblUserMessage.Visible = false;

            return true;
        }

        private bool _HandlePreviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannnot apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch(_TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                                        
                        //no previous test required
                        lblUserMessage.Visible = false;
                        return true;
                case clsTestType.enTestType.WrittenTest:
                        {
                            if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                            {
                                lblUserMessage.Visible = true;
                                lblUserMessage.Text = "Person must pass the Vision Test before applying for Written Test";
                                btnSave.Enabled = false;
                                dtpTestDate.Enabled = false;
                                return false;
                            }
                            else
                            {
                                lblUserMessage.Visible = false;
                                btnSave.Enabled = true; 
                                dtpTestDate.Enabled = true;
                                return true;
                            }
                        }
                case clsTestType.enTestType.StreetTest:
                    {
                        if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                        {
                            lblUserMessage.Visible = true;
                            lblUserMessage.Text = "Person must pass the Written Test before applying for Street Test";
                            btnSave.Enabled = false;
                            dtpTestDate.Enabled = false;
                            return false;
                        }
                        else
                        {
                            lblUserMessage.Visible = false;
                            btnSave.Enabled = true;
                            dtpTestDate.Enabled = true;
                            return true;

                        }
                    }

            }
            return true;

        }

        private bool _HandleRetakeApplication()
        {
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                clsApplication newApplication = new clsApplication();

                newApplication.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                newApplication.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                newApplication.ApplicationDate = DateTime.Now;
                newApplication.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                newApplication.PaidFees = (float)clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees;
                newApplication.LastStatusDate = DateTime.Now;
                newApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if (!newApplication.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = newApplication.ApplicationID;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gbTestType_Enter(object sender, EventArgs e)
        {

        }
    }
}
