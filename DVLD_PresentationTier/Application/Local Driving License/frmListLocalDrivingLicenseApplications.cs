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
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        private DataTable _dtLocalDrivingLicenseApplications;
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }



        private void frmLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();

            dgvLocalDrivingLicenseApplications.DataSource = _dtLocalDrivingLicenseApplications;

            cbFilterBy.SelectedIndex = 0;

            lblRecords.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();

            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0 )
            {
                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 100;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 250;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns[2].Width = 110;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 140;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns[6].HeaderText = "Status";
                dgvLocalDrivingLicenseApplications.Columns[6].Width = 80;

            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible )
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = "";

            switch (cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    ColumnFilter = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No":
                    ColumnFilter = "NationalNo";
                    break;
                case "Full Name":
                    ColumnFilter = "FullName";
                    break;
                case "Status":
                    ColumnFilter = "Status";
                    break;
            }


            //Reset the filters in case nothing selected or filter value contain nothing
            if (txtFilterValue.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecords.Text = _dtLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }

            if (ColumnFilter == "LocalDrivingLicenseApplicationID")
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", ColumnFilter, txtFilterValue.Text);
            }
            else
            {
                _dtLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", ColumnFilter, txtFilterValue.Text);
            }

            lblRecords.Text = _dtLocalDrivingLicenseApplications.DefaultView.Count.ToString();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.L.AppID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
            //if (txtFilterValue.Text.Trim() == "")
            //{
            //    frmLocalDrivingLicenseApplications_Load(null, null);
            //}
        }

     

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplications frm = new frmAddUpdateLocalDrivingLicenseApplications();
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }

   

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //clsApplication Application = clsApplication.FindBaseApplication(
            //    (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            //Application.Cancel();

            if (MessageBox.Show("Are you sure you want to cancel this Application","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLocalDrivingLicenseApplication LocalDrivingLicense = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID
                ((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            //if (LocalDrivingLicense.ApplicationStatus == clsApplication.enApplicationStatus.Cancelled)
            //{
            //    MessageBox.Show("this application actualy is canceled choose another Application","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            if (LocalDrivingLicense.Cancel())
            {
                MessageBox.Show("Local Driving License Application is Canceled successfully","Canceled",MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmLocalDrivingLicenseApplications_Load(null, null);
            }
            else
            {
                MessageBox.Show("Could not cancel application","Error",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
            }

        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm = new frmLocalDrivingLicenseApplicationInfo((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplications frm = new frmAddUpdateLocalDrivingLicenseApplications((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmLocalDrivingLicenseApplications_Load(null, null);

        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this Application? ","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            } 
            
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells [0].Value;


            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID
                (LocalDrivingLicenseApplicationID);

            if (localDrivingLicenseApplication != null)
            {
                if (localDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Delete successfully.","Deleted",MessageBoxButtons.OK,MessageBoxIcon.Information);

                    frmLocalDrivingLicenseApplications_Load(null, null);

                }
                else
                {
                    MessageBox.Show("Could not delete application, other depends on it.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }

        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID
                (LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalDrivingLicenseApplication.isLicenseIssued();

            //Enable only if person passed all Tests and does not have license
            IssueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;

            showLicenseToolStripMenuItem.Enabled = LicenseExists;
            editApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New) && !LicenseExists;
            sechduleTestsMenue.Enabled = !LicenseExists;


            //We only cancle the application with status = new
            cancelApplicationToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New;

            //We only allow delete incase the application status is new not Complete or Cancel
            deleteApplicationToolStripMenuItem.Enabled = LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New;


            //Enable Disable schedule menue and it's sub menue
            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest);
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            sechduleTestsMenue.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);


            if (sechduleTestsMenue.Enabled)
            {
                //to allow schedule vision test, Person must not passed the same test before.
                tsmiScheduleVisionTest.Enabled = !PassedVisionTest;

                //to allow schedule written test, Person must passed vision test but not passed written test before.
                tsmiScheduleWrittenTest.Enabled = PassedVisionTest && !PassedWrittenTest;

                //To Allow Schdule steet test, Person must pass the vision * written tests, and must not passed the same test before
                tsmiScheduleStreetTest.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }


        }

        private void issueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDriverLicenseForTheFirstTime frm = new frmIssueDriverLicenseForTheFirstTime((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }

        private void tsmiScheduleVisionTest_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value,clsTestType.enTestType.VisionTest);
            frm.ShowDialog();

            frmLocalDrivingLicenseApplications_Load(null, null);
        }

        private void tsmiScheduleWrittenTest_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }

        private void tsmiScheduleStreetTest_Click(object sender, EventArgs e)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestType.enTestType.StreetTest);
            frm.ShowDialog();
            frmLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            int LicenseID = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(
               LocalDrivingLicenseApplicationID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
                frm.ShowDialog();

            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            int PersonID = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(
               LocalDrivingLicenseApplicationID).ApplicantPersonID;

            if (PersonID != -1)
            {
                frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Person Found!", "No Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
