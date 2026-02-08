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
    public partial class ctrUserCard : UserControl
    {
        private clsUser _User;

        private int _UserID = -1;

        public int UserID
        {
            get { return _UserID; }
        }
        public ctrUserCard()
        {
            InitializeComponent();
        }


        private void _ResetUserInfo()
        {
            lblUserID.Text = "???";
            lblUserName.Text = "???";
            lblIsActive.Text = "???";
        }

        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.FindByUserID(UserID);

            if (_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show("No User with ID = " + UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);


                return;
            }
            _FillUserInfo();
        }

        private void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            _UserID = _User.UserID;
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;

            if (_User.IsActive)
            {
                lblIsActive.Text = "Yes";
            }
            else
            {
                lblIsActive.Text = "No";
            }

        }
    }
}



