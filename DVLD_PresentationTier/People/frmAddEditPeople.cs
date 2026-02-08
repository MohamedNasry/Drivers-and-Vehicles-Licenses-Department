using DVLD_PresentationTier.Properties;
using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DVLD_PresentationTier
{
    public partial class frmAddEditPeople : Form
    {
        //Declar a delegate 
        public delegate void DataBackEventHandler(object sender, int PersonID);

        //Declare an event using the delegate
        public event DataBackEventHandler DataBack;
        public enum enMode { AddNew = 0, Update = 1}
        public enum enGendor { Female = 0, Male = 1}

        private enMode _Mode;

        private int _PersonID;

        private clsPerson _Person;

        public frmAddEditPeople()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }
        public frmAddEditPeople(int PersonID)
        {
            InitializeComponent();

            _Mode = enMode.Update;

            _PersonID = PersonID;

            
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach( DataRow row in dtCountries.Rows )
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetDefaultValues()
        {
            _FillCountriesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblMode.Text = "Update Person";
            }


            //Set Default image for the person
            if ( rbMale.Checked )
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }

            llRemove.Visible = (pbPersonImage.ImageLocation != null);

            //We set the max Date to 18 years from today
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);

            //Should not allow adding age more than 100 years
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            //this will set defaul country for user
            cbCountry.SelectedIndex = cbCountry.FindString("Morocco");

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
         
            

        }
        private void _LoadData()
        {
            
            _Person = clsPerson.Find(_PersonID);

            if ( _Person == null )
            {
                MessageBox.Show("No Person with ID = " + _PersonID,"Person Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();

                return;
            }

       
            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;


            if ( _Person.Gendor == 1)
            {
                rbMale.Checked = true;   
            }
            else
            {
                rbFemale.Checked = true;     
            }

            string ImagePath = _Person.ImagePath;

            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                }
                else
                {
                    MessageBox.Show("Could not Found This image : " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
          

            llRemove.Visible = (_Person.ImagePath != "");

            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;

            //this will select the country in the combobox.
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);

        }

        private void frmAddEditPeople_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            
            if ( _Mode == enMode.Update )
            {
                _LoadData();
            }
            
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            //change the default Image to male incase there is no image set

            if (pbPersonImage.ImageLocation == null) 
            {
                pbPersonImage.Image = Resources.Male_512;
            }
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            //change the default image to female incase there is no image set

            if (pbPersonImage.ImageLocation == null)
            {
                pbPersonImage.Image = Resources.Female_512;
            }

            
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                //MessageBox.Show("Selected Image is:" + selectedFilePath);

                pbPersonImage.Load(selectedFilePath);
                llRemove.Visible = true;
                // ...
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        //دورها: تشغيل جميع أحداث Validating ديال الـ Controls اللي عندها الخاصية CausesValidation = true
        //بمعنى: كتنادي Validating Event على كل TextBox, ComboBox
            if (!this.ValidateChildren())
            {
                //here we dont continue because the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon to show the reason of error");
                return;
            }


            if (!_HandlePersonImage())
                return;

            


            int NationalityCountryID = clsCountry.Find(cbCountry.Text).CountryID;

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();

            _Person.LastName = txtLastName.Text.Trim();

            _Person.Email = txtEmail.Text.Trim();

            _Person.Phone = txtPhone.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Address = txtAddress.Text.Trim();
            _Person.NationalityCountryID = NationalityCountryID;
            if (rbFemale.Checked)
            {
                _Person.Gendor = (byte)enGendor.Female;
            }
            else
            {
                _Person.Gendor = (byte)enGendor.Male;
            }

            if (pbPersonImage.ImageLocation != null)
            {
                _Person.ImagePath = pbPersonImage.ImageLocation;
            }
            else
            {
                _Person.ImagePath = "";
            }

            if (_Person.Save())
            {
                //Change form load to update
                _Mode = enMode.Update;

                lblMode.Text = "Update Person";

                lblPersonID.Text = _Person.PersonID.ToString();

                MessageBox.Show("Data Saved Successfully.","Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);

                //trigger the event to send data back to the caller form

                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
               

          
        }

        private void dtpDateOfBirth_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNationalNo.Text))
            {
                if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.IsPersonExist(txtNationalNo.Text.Trim()))
                {
                    e.Cancel = true;
                    //txtNationalNo.Focus();
                    errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
                }
                else
                {
                   // e.Cancel = false;
                    errorProvider1.SetError(txtNationalNo, null);
                }

            }
            else
            {
                e.Cancel = true;
                //txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "This field is required!");
            }
        }

        //private void txtFirstName_Validating(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtFirstName.Text))
        //    {
        //        e.Cancel = true;
        //        txtFirstName.Focus();
        //        errorProvider1.SetError(txtFirstName, "First Name Should have a value!");
        //    }
        //    else
        //    {
        //        e.Cancel = false;
        //        errorProvider1.SetError(txtFirstName, "");
        //    }
        //}

        //private void txtSecondName_Validating(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty (txtSecondName.Text))
        //    {
        //        e.Cancel = true;
        //        txtSecondName.Focus();
        //        errorProvider1.SetError(txtSecondName, "Second Name Should have a value!");
        //    }
        //    else
        //    {
        //        e.Cancel = false;
        //        errorProvider1.SetError(txtSecondName, "");
        //    }
        //}

        //private void txtLastName_Validating(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtLastName.Text))
        //    {
        //        e.Cancel = true;
        //        txtLastName.Focus();
        //        errorProvider1.SetError(txtLastName, "Last Name Should have a value!");

        //    }
        //    else
        //    {
        //        e.Cancel = false;
        //        errorProvider1.SetError(txtLastName, "");
        //    }
        //}

        //private void txtPhone_Validating(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtPhone.Text))
        //    {
        //        e.Cancel= true;
        //        txtPhone.Focus();
        //        errorProvider1.SetError(txtPhone, "Phone Should have a value!");
        //    }
        //    else
        //    {
        //        e.Cancel = false;
        //        errorProvider1.SetError(txtPhone, "");
        //    }
        //}

        //private void txtAddress_Validating(object sender, CancelEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtAddress.Text))
        //    {
        //        e.Cancel = true;
        //        txtAddress.Focus();
        //        errorProvider1.SetError(txtAddress, "Address Should have a value!");
        //    }
        //    else
        //    {
        //        e.Cancel = false;
        //        errorProvider1.SetError(txtAddress, "");
        //    }

        //}

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            //First: Set Auto Validate Property of your form to EnableAllowFocusChange in design
            TextBox Temp = (TextBox)sender;

            if (string.IsNullOrEmpty(Temp.Text))
            {
                e.Cancel = true;
               
                errorProvider1.SetError(Temp, "This Field is required!");
            }
            else
            {
                // e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }

        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            //no need to validate email incase  it's empty
            if (txtEmail.Text.Trim() == "")
            {
                return;
            }
           
            if (clsValidation.ValidateEmail(txtEmail.Text))
            {
               // e.Cancel = false;
                errorProvider1.SetError(txtEmail, null);
            }
            else
            {
                e.Cancel = true;
               
                errorProvider1.SetError(txtEmail, "Invalid email address Format!");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void llRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if (rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }

            llRemove.Visible = false;
        }

        private bool _HandlePersonImage()
        {
            //this procedure will handle the person image,
            //it will take care: يتولى of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and
            //place it in the images folder


            //Check if image changed or Not 
            if (_Person.ImagePath != pbPersonImage.ImageLocation)//mean picture is change
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        //first we delete the olde image from the forder
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {

                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                    //then we copy the new image to the image forder
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageTpProjectImagesForder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;

                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error: Copying Image File","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
