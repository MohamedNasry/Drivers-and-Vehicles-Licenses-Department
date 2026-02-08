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
    public partial class frmUpdateTestType : Form
    {
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        private clsTestType _TestType;
        public frmUpdateTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = TestTypeID;
        }

       

        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(_TestTypeID);

            if ( _TestType != null )
            {
                lblTestTypeID.Text = ((int)_TestType.TestTypeID).ToString();
                txtTitle.Text = _TestType.TestTypeTitle;
                txtDescription.Text = _TestType.TestTypeDescription;
                txtFees.Text = _TestType.TestTypeFees.ToString();
            }
            else
            {
                MessageBox.Show("Could not Found Test Type With ID = " + (int)_TestTypeID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //here we dont continue because the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon to show the reason of error",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _TestType.TestTypeTitle = txtTitle.Text.Trim();
            _TestType.TestTypeDescription = txtDescription.Text.Trim();
            _TestType.TestTypeFees = Convert.ToDecimal(txtFees.Text.Trim());

            if (_TestType.UpdateTestType())
            {
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Error : Data not Saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title could not be blank!");

            }
            else
            {
                errorProvider1.SetError(txtTitle, "");
            }
        }

        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescription, "Title could not be blank");

            }
            else
            {
                errorProvider1.SetError(txtDescription, "");
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees could not be blank");
                return;

            }
            else
            {
                errorProvider1.SetError(txtFees, "");
            }

            if (!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFees, "");
            }
        }
    }
}
