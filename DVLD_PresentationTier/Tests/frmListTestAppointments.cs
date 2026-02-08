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
using static DVLDBusinessLayer.clsTestType;

namespace DVLD_PresentationTier
{
    public partial class frmListTestAppointments : Form
    {
        private clsTestType.enTestType _TestTypeID;
        private static DataTable _dtLicenseTestAppointments;
        private int _LocalDrivingLicenseApplicationID;

        private DataTable _dtAppointments;
        public frmListTestAppointments(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
           _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            _TestTypeID = TestTypeID;

    
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    pbTestTypeImage.Image = Resources.Vision_512;
                    lblTitle.Text = "Vision Test Appointments";
                    break;
                case clsTestType.enTestType.WrittenTest:
                    pbTestTypeImage.Image = Resources.Written_Test_512;
                    lblTitle.Text = "Written Test Appointments";
                    break;
                case clsTestType.enTestType.StreetTest:
                    pbTestTypeImage.Image = Resources.driving_test_512;
                    lblTitle.Text = "Street Test Appointments";
                    break;
            }
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //----------------------------------------------------------------------------------------
            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);

            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
                frm1.ShowDialog();
                frmListTestAppointments_Load(null, null);
                return;
            }

            //if person already passed the test s/he cannot retak it.
            if (LastTest.TestResult == 1)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            frmScheduleTest frm2 = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
            frm2.ShowDialog();

            frmListTestAppointments_Load(null, null);

        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();

            ctrDrivingLicenseApplicationInfo1.LoadLocalDrivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);

            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);


            dgvAppointments.DataSource = _dtLicenseTestAppointments;
            lblCount.Text = dgvAppointments.Rows.Count.ToString();

            if (dgvAppointments.Rows.Count > 0)
            {
                dgvAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvAppointments.Columns[0].Width = 150;

                dgvAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvAppointments.Columns[1].Width = 200;

                dgvAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvAppointments.Columns[2].Width = 150;

                dgvAppointments.Columns[3].HeaderText = "Is Locked";
                dgvAppointments.Columns[3].Width = 100;
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvAppointments.CurrentRow.Cells[0].Value;

            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID, TestAppointmentID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTakeTest frm = new frmTakeTest((int)dgvAppointments.CurrentRow.Cells[0].Value, _TestTypeID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }
    }
}
