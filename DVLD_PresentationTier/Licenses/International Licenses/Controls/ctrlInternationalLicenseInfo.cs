using DVLD_PresentationTier.Properties;
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
using System.IO;

namespace DVLD_PresentationTier
{
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID;

        private clsInternationalLicense _InternationalLicense;

        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }

        public clsInternationalLicense SelectedInternationalLicenseInfo
        { get { return _InternationalLicense; } }

        private void _LoadPersonImage()
        {
            if (_InternationalLicense.DriverInfo.PersonInfo.Gendor == 1)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            string ImagePath = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        public void LoadInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseID);
            if (_InternationalLicense == null)
            {
                MessageBox.Show("Could not find International License ID = " + _InternationalLicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblInterLicenseID.Text = _InternationalLicenseID.ToString();
            lblFullName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo;
            lblIssuedUsingLocalLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();
            lblGendor.Text = _InternationalLicense.DriverInfo.PersonInfo.Gendor == 1 ? "Male" : "Female";
            lblIssueDate.Text = clsFormat.DateToShort(_InternationalLicense.IssueDate);
            lblExpirationDate.Text = clsFormat.DateToShort(_InternationalLicense.ExpirationDate);
            lblDateOfBirth.Text = clsFormat.DateToShort(_InternationalLicense.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text = _InternationalLicense.DriverID.ToString();   
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblApplicationID.Text = _InternationalLicense.ApplicantID.ToString();
            _LoadPersonImage();
        }


    }
}
