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
    public partial class frmChangePassword : Form
    {
        private int _CurrentUserID = -1;

        private clsUser _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            _CurrentUserID = UserID;

        }

        private void _ReseteDefaultValue()
        {
            txtCurrentPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtNewPassword.Text = "";
            txtCurrentPassword.Focus(); 
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ReseteDefaultValue();

            _User = clsUser.FindByUserID(_CurrentUserID);

            if (_User == null )
            {
                //Here we dont continue because the form is not valid
                MessageBox.Show("Could not found user with ID "+ _CurrentUserID,"Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            ctrUserCard1.LoadUserInfo(_CurrentUserID);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password Cannot be blank!");

            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, "");
            }
            if (txtCurrentPassword.Text.Trim() != _User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password is Wrong!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, "");
            }
            
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password cannot be blank!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtNewPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match!");
            }
            else
            {
         
                errorProvider1.SetError(txtConfirmPassword, null);
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

            if (_User.Password == txtNewPassword.Text)
            {
                MessageBox.Show("New Password is Match the Old Password","Validation",
                    MessageBoxButtons.OK,MessageBoxIcon.Error); return;
            }
            _User.Password = txtNewPassword.Text;

            if (_User.Save())
            {
                MessageBox.Show("Password Changed Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ReseteDefaultValue();
            }
            else
            {
                MessageBox.Show("An Error Occured, Password did Not change.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



            //if (clsUser.ChangePassword(_CurrentUserID, txtConfirmPassword.Text))
            //{
            //    MessageBox.Show("Password Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{

            //    MessageBox.Show("Password not Saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
