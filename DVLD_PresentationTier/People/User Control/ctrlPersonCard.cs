using DVLD_PresentationTier.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationTier
{
    public partial class ctrlPersonCard : UserControl
    {
        private clsPerson _Person;

        private int _PersonID = -1;

        public int PersonID
        {
            get { return _PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }
        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {
            //if (this.DesignMode)
            //    return;

            _Person = clsPerson.Find(PersonID);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with ID = " + PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);


                return;
            }

            _FillPersonInfo();

            //lblPersonID.Text = PersonID.ToString();
            //lblNationalNo.Text = _Person.NationalNo;
            //lblName.Text = _Person.FullName;
            //lblDateOfBirth.Text = _Person.DateOfBirth.ToString();

            //if (_Person.Gendor == 1)
            //{
            //    lblGendor.Text = "Male";
            //    pbPersonImage.Image = Resources.Male_512;
            //}
            //else
            //{
            //    lblGendor.Text = "Female";
            //    pbPersonImage.Image = Resources.Female_512;
            //}

            //lblAddress.Text = _Person.Address;
            //lblPhone.Text = _Person.Phone;
            //lblEmail.Text = _Person.Email;
            //lblCountry.Text = _Person.CountryInfo.CountryName;

            //if (_Person.ImagePath != "")
            //{
            //    pbPersonImage.ImageLocation = _Person.ImagePath;
            //}
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National Number = " + NationalNo, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);


                return;
            }
            _FillPersonInfo();
        }
        public void ResetPersonInfo()
        {
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblName.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblGendor.Text = "[????]";
            lblCountry.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblAddress.Text = "[????]";

            pbGendor.Image = Resources.Male_30px;
            pbPersonImage.Image = Resources.Male_512;
        }

        private void _LoadPersonImage()
        {
            if (_Person.Gendor == 1)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                {
                    pbPersonImage.ImageLocation = ImagePath;
                }
                else
                {
                    MessageBox.Show("Could not Found This image : " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void _FillPersonInfo()
        {
            llEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblName.Text = _Person.FullName;
            if (_Person.Gendor == 1)
            {
                lblGendor.Text = "Male";
                pbGendor.Image = Resources.Male_30px;
            }
            else
            {
                lblGendor.Text = "Female";
                pbGendor.Image = Resources.Female_32px;
            }
                //lblGendor.Text = _Person.Gendor == 1 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblPhone.Text = _Person.Phone;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text = _Person.CountryInfo.CountryName;

            _LoadPersonImage();
        }

      
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPeople frm = new frmAddEditPeople(_PersonID);
            frm.ShowDialog();

            //refresh
            LoadPersonInfo(_PersonID);
        }

        private void pbPersonImage_Click(object sender, EventArgs e)
        {

        }
    }
}
