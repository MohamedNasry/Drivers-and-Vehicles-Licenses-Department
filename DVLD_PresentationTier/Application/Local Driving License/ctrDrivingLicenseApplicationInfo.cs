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
    public partial class ctrDrivingLicenseApplicationInfo : UserControl
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private int _LocalDrivingLicenseApplicationID = -1;

        private int _LicenseID = -1;

        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }
        //public clsLocalDrivingLicenseApplication SelectedLocalDrivingLicenseApplication
        //{
        //    get { return _LocalDrivingLicenseApplication; }
        //}
        public ctrDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        public void LoadLocalDrivingLicenseApplicationInfo(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);


            if (_LocalDrivingLicenseApplication == null )
            {
                ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show("No Local Driving License Application with ID, "+ LocalDrivingLicenseApplicationID,"Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _FillLocalDrivingLicenseApplicationInfo();


        }

        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                ResetLocalDrivingLicenseApplicationInfo();


                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        public void ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplicationID = -1;
            lblDLAppID.Text = "[???]";
            lblAppliedForLicense.Text = "[???]";
            lblPassedTests.Text = "[???]";
            ctrlApplicationBasicInfo1.ResetApplicationInfo();   
       
        }




        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();

            //in case ther is license
            llShowLicenseInfo.Enabled = (_LicenseID != -1);

            _LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            lblDLAppID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblAppliedForLicense.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;

            lblPassedTests.Text = _LocalDrivingLicenseApplication.GetPassedTestCount().ToString() + "/3";
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);





        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //call form to show license info
        }
    }
}
