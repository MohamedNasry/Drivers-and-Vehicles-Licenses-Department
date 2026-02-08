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
    public partial class frmInternationalLicenseApplication : Form
    {
        private int _InternationalLicenseID = -1;
        public frmInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FilterFocus();

            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));//add one year.   

            llShowLicensesHistory.Enabled = false;
            llShowLicensesInfo.Enabled = false;
            btnIssue.Enabled = false;

            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            lblLocalLicenseID.Text = SelectedLicenseID.ToString();

            llShowLicensesHistory.Enabled = SelectedLicenseID != -1;

            if (SelectedLicenseID == -1)
            {

                return;
            }

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is Expired, choose a valid license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.ClassName != "Class 3 - Ordinary driving license")
            {
                MessageBox.Show("Selected License should be license 3"
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }
            //check if person already have an active international license
            int ActiveInternaionalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);

            if (ActiveInternaionalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternaionalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicensesInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternaionalLicenseID;
                btnIssue.Enabled = false;
                return;
            }

  
            btnIssue.Enabled = true;

        }

        private void btnIssue_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }






            clsInternationalLicense InternationalLicense = new clsInternationalLicense();
            //those are the information for the base application, because it inhirts from application, they are part of the sub class.

            InternationalLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonId;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFees = (float)clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees;
            


            InternationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);

            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = InternationalLicense.ApplicantID.ToString();
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssue.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicensesInfo.Enabled = true;
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm =
            new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonId);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void llShowLicensesInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}
