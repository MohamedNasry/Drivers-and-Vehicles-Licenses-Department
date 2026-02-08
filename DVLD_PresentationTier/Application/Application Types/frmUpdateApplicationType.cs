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
    public partial class frmUpdateApplicationType : Form
    {
        private int _AppTypeID = -1;

        clsApplicationType _AppType;
        public frmUpdateApplicationType(int AppTypeID)
        {
            InitializeComponent();
            _AppTypeID = AppTypeID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //here we dont continue because the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon to show the reason of error",
                    "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            _AppType.ApplicationTypeTitle = txtAppTypeTitle.Text.Trim();
            _AppType.ApplicationFees = Convert.ToDecimal(txtAppTypeFees.Text.Trim());

            if (_AppType.UpdateApplicationType())
            {
                MessageBox.Show("Data Saved Successfully","Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Error : Data not Saved Successfully","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
           


        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _AppType = clsApplicationType.Find(_AppTypeID);

            if (_AppType != null)
            {
                lblAppTypeID.Text = _AppType.ApplicationTypeID.ToString();
                txtAppTypeTitle.Text = _AppType.ApplicationTypeTitle;
                txtAppTypeFees.Text = _AppType.ApplicationFees.ToString();
            }
            else
            {
                MessageBox.Show("An Error Ouccur,","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtAppTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAppTypeTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtAppTypeTitle, "Title could not be blank");

            }
            else
            {
                errorProvider1.SetError(txtAppTypeTitle, "");
            }
        }

        

       

        private void txtAppTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAppTypeFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtAppTypeFees, "Fees could not be blank");
                return;

            }
            else
            {
                errorProvider1.SetError(txtAppTypeFees, "");
            }

            if (!clsValidation.IsNumber(txtAppTypeFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtAppTypeFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtAppTypeFees, "");
            }
        }
    }
}
