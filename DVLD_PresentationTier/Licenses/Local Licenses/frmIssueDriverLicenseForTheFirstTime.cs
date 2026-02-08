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
    public partial class frmIssueDriverLicenseForTheFirstTime : Form
    {
        int _LocalDrivingLicenseApplicationID = -1;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmIssueDriverLicenseForTheFirstTime(int LocalDrivingLicenseApplication)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplication;
        }

        private void frmIssueDriverLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {

                MessageBox.Show("No Applicaiton with ID=" + _LocalDrivingLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            if (!_LocalDrivingLicenseApplication.isPassedAllTests())
            {

                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            if (LicenseID != -1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }


            ctrDrivingLicenseApplicationInfo1.LoadLocalDrivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);



        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int LicenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirstTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("Driving License Issued Successfully with License ID=" + LicenseID.ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Error Issuing Driving License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
