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
    public partial class frmAddUpdateUser : Form
    {

        //Declar a delegate 
        public delegate void DataBackEventHandler(object sender, int UserID);

        //Declare an event using the delegate
        public event DataBackEventHandler DataBack;

        public enum enMode { AddNew = 0, Update = 1}

        private enMode _Mode;

        private int _UserID;

        private clsUser _User;
        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            _Mode = enMode.Update;
        }

        private void _ResetDefaultValues()
        {
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";

                _User = new clsUser();

                tbLoginInfo.Enabled = false;

                btnSave.Enabled = false;

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                tbLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
        }

        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if ( _User == null )
            {
                MessageBox.Show("No User with ID = " + _UserID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo( _User.PersonID );

        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tbLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tbLoginInfo;
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                if (clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person Already has a user, choose another one", "Selected another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();

                }
                else
                {
                    btnSave.Enabled = true;
                    tbLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tbLoginInfo;
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
       
       
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                if (txtUserName.Text.Trim() == _User.UserName && clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;

                    errorProvider1.SetError(txtUserName, "this Username Already used!");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, "");
                }
                e.Cancel = true;

                errorProvider1.SetError(txtUserName, "UserName cannot be blank!");
            }
            else
            {
                errorProvider1.SetError(txtUserName, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtPassword, "Password Cannot be blank!");
            }
            else
            {
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                e.Cancel = true;

                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");

            }
            else
            {
               // e.Cancel = false;

                errorProvider1.SetError(txtConfirmPassword, "");
            }    
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
             
            if (!this.ValidateChildren())
            {
                //here we dont continue because the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon to show the reason of error",
                    "Validation Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

                return;
            }

            // _User = new clsUser();

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            if (chkIsActive.Checked)
            {
                _User.IsActive = true;
            }
            else
            {
                _User.IsActive = false;
            }

            if (_User.Save())
            {
                _Mode = enMode.Update;

                lblTitle.Text = "Update User";
                this.Text = "Update User";

                lblUserID.Text = _User.UserID.ToString();

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //trigger the event to send data back to the caller form

                DataBack?.Invoke(this, _User.UserID);
            }
            else
            {
                MessageBox.Show("Error: Data is not saved successfully", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {

        }
    }
}

