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
    public partial class frmAddUpdateLocalDrivingLicenseApplications : Form
    {
        public enum enMode { AddNew = 0, Update =1}

        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;

        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

       // clsLicenseClass _LicenseClass;
        public frmAddUpdateLocalDrivingLicenseApplications()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;

            
        }

        public frmAddUpdateLocalDrivingLicenseApplications(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _Mode = enMode.Update;
        }

        private void _FillClassesNamesInComboBox()
        {
            DataTable dtClassesNames = clsLicenseClass.GetAllLicenseClasses();

            foreach ( DataRow row in dtClassesNames.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }


        }


        private void _ResetDefaultValues()
        {
           tbApplicationInfo.Enabled = false;
            btnSave.Enabled = false;

            //lblDLApplicationID.Text = "[???]";
            //lblApplicationDate.Text = DateTime.Now.ToString();
            //lblApplicationFees.Text = "15";
            //lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            _FillClassesNamesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFocus();
                tbApplicationInfo.Enabled = false;

                cbLicenseClass.SelectedIndex = 2;
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).ApplicationFees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tbApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }

        }


        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
            lblLDLApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblApplicationFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedBy.Text = clsUser.FindByUserID(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;




        }

        private void frmNewLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            ctrlPersonCardWithFilter1.OnPersonSelected += ctrlPersonCardWithFilter1_OnPersonSelected;

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //tcLocalDrivingLAInfo.SelectedTab = tbApplicationInfo;

            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tbApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tbApplicationInfo"];
                return;
            }


            //incase of add new mode.
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                btnSave.Enabled = true;
                tbApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tbApplicationInfo"];

            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void cbLicenseClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_LicenseClass = clsLicenseClass.Find(cbLicenseClass.Text.Trim());
        }

        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received
            _SelectedPersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(PersonID);


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }


            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;


            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);



            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }

            //check if user already have issued license of the same driving  class.
            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {

                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            if (_Mode == enMode.AddNew)
                _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;

            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblApplicationFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;


            byte MinAllowedAge = clsLicenseClass.Find(LicenseClassID).MinimumAllowedAge;
            DateTime dateOfBirth = clsPerson.Find(_SelectedPersonID).DateOfBirth;
            int age = DateTime.Now.Year - dateOfBirth.Year;
            if (MinAllowedAge > Convert.ToByte(age))
            {
                MessageBox.Show($"Person is not allowed for this Driving License Class, it requires a {MinAllowedAge} years old and above", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_LocalDrivingLicenseApplication.Save())
            {
                lblLDLApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();

                //Change form mode to update
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void frmAddUpdateLocalDrivingLicenseApplications_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
