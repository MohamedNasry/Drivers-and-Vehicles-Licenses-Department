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
    public partial class frmReplaceLostOrDamagedLicense : Form
    {
        private int _NewLicenseID = -1;
        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }

        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FilterFocus();

            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
   

            llShowLicensesHistory.Enabled = false;
            llShowNewLicensesInfo.Enabled = false;
            btnIssueReplacement.Enabled = false;

           
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;


            rbDamagedLicense.Checked = true;
            
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            lblOldLicenseID.Text = SelectedLicenseID.ToString();

            llShowLicensesHistory.Enabled = SelectedLicenseID != -1;

            if (SelectedLicenseID == -1)
            {

                return;
            }




            //check the license is not Active.
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }

            btnIssueReplacement.Enabled = true;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense).ApplicationFees.ToString();
            
            lblTitle.Text = "Replacement For Damaged License";

            this.Text = "Replacement For Damaged License";
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReplaceLostDrivingLicense).ApplicationFees.ToString();

            lblTitle.Text = "Replacement For Lost License";
            this.Text = "Replacement For Lost License";
        }

        private void llShowNewLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Replace the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(
                rbDamagedLicense.Checked ? clsLicense.enIssueReason.DamagedReplacement : clsLicense.enIssueReason.LostReplacement,
                clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Replace the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblReplacedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("Licensed Replaced Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueReplacement.Enabled = false;
            gbReplacementFor.Enabled = false;   
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowNewLicensesInfo.Enabled = true;


        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
