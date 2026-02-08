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
    public partial class frmLogin : Form
    {


       
        public frmLogin()
        {
            InitializeComponent();
        }

 
        private void btnLogin_Click(object sender, EventArgs e)
        {
           
            clsUser User = clsUser.FindByUserNameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());

            if (User != null)
            {
                if (chKRememberMe.Checked)
                {
                    //store username and password
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());

                }
                else
                {
                    //store empty username and password
                    clsGlobal.RememberUsernameAndPassword("", "");

                }
                //incase the user is not active
                if (!User.IsActive)
                {

                    txtUserName.Focus();
                    MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = User;
                this.Hide();
                Main frm = new Main(this);
                frm.ShowDialog();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //if (CurrentUser == null )
            //{
            //    MessageBox.Show("Invalid Username/Password", "Wrong Credentials",MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);
            //    _ReseteDefaultValue();  
            //    return;
            //}

            //if (!CurrentUser.IsActive)
            //{
            //    MessageBox.Show("Username is not active", "Wrong Credentials", MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);
            //    _ReseteDefaultValue();
            //    return;
            //}

            //clsGlobal.CurrentUser = CurrentUser;

            //Main frm = new Main();
            //frm.ShowDialog();

            //if (chKRememberMe.Checked)
            //{
            //    txtUserName.Text = _UserName;
            //    txtPassword.Text = _Password;
            //    chKRememberMe.Checked = true;
            //}
            //else
            //{
            //    _ReseteDefaultValue();
            //}

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";

            if (clsGlobal.GetStoredGredential(ref Username,ref Password) )
            {
                txtPassword.Text = Password;
                txtUserName.Text = Username;
                chKRememberMe.Checked = true;
            }
            else
            {
                chKRememberMe.Checked = false;
            }
        }
    }
}
