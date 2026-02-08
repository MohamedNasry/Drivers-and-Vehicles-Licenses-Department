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
    public partial class frmDetainLicense : Form
    {
        
        private int _LicenseID = -1;
        public frmDetainLicense()
        {
            InitializeComponent();
        }

        public frmDetainLicense(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
        }
        private void frmDetainLicense_Load(object sender, EventArgs e)
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
                btnDetain.Enabled = false;
            }
            
            
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            llShowLicensesHistory.Enabled = _LicenseID != -1;
            if (_LicenseID == -1)
            {
                return;
            }

            lblLicenseID.Text = _LicenseID.ToString();

            if (clsDetainedLicense.isLicenseDetained(_LicenseID))
            {
                MessageBox.Show("This license is already detained.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }

            txtFineFees.Focus();
            btnDetain.Enabled = true;


        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                return;
            }

            int DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(double.Parse(txtFineFees.Text), clsGlobal.CurrentUser.UserID);

            if (DetainID == -1)
            {
                MessageBox.Show("Failed to detain the license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblDetainID.Text = DetainID.ToString();

            MessageBox.Show("License detained successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnDetain.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            txtFineFees.Enabled = false;
            llShowNewLicensesInfo.Enabled = true;




        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text))
            {
                errorProvider1.SetError(txtFineFees, "Fine fees is required.");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtFineFees, string.Empty);
            }
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
          
             e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
           
        }

        private void llShowNewLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonId);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
