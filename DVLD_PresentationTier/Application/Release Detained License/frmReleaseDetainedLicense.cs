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
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _LicenseID = -1;
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            if (_LicenseID != -1)
            {
                ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_LicenseID);
                lblLicenseID.Text = _LicenseID.ToString();
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            }
            else
            {
                ctrlDriverLicenseInfoWithFilter1.FilterFocus();
                llShowLicensesHistory.Enabled = false;
                llShowNewLicensesInfo.Enabled = false;
                btnRelease.Enabled = false;
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            llShowLicensesHistory.Enabled = _LicenseID != -1;
            if (_LicenseID == -1)
            {
                return;
            }


            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                lblLicenseID.Text = _LicenseID.ToString();
                lblDetainID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainID.ToString();
                lblDetainDate.Text = clsFormat.DateToShort(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainDate);
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense).ApplicationFees.ToString("0.00");
                lblCreatedBy.Text = clsUser.FindByUserID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.CreatedByUserID).UserName;
                lblFineFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.FineFees.ToString("0.00");

                lblTotalFees.Text = (Convert.ToDecimal(lblApplicationFees.Text) + Convert.ToDecimal(lblFineFees.Text)).ToString("0.00");

                btnRelease.Enabled = true;
            }
            else
            {
                MessageBox.Show("Selected license is not detained.", "Invalid License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRelease.Enabled = false;
            }
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this license?", "Confirm Release", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int ApplicationID = -1;

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID, ref ApplicationID))
            {
                lblApplicationID.Text = ApplicationID.ToString();
                MessageBox.Show("License released successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                llShowNewLicensesInfo.Enabled = true;
                ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
                btnRelease.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed to release the license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonId);
            frm.ShowDialog();
        }

        private void llShowNewLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }
    }
}