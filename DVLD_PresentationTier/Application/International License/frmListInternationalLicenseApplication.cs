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
    public partial class frmListInternationalLicenseApplication : Form
    {
        private DataTable _dtInternationalLicenseApplications;
        public frmListInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmListInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            _dtInternationalLicenseApplications = clsInternationalLicense.GetAllInternationalLicenses();

            dgvListInternationalLicense.DataSource = _dtInternationalLicenseApplications;

            cbFilterBy.SelectedIndex = 0;

            lblRecords.Text = dgvListInternationalLicense.Rows.Count.ToString();

            if (dgvListInternationalLicense.Rows.Count > 0)
            {
                dgvListInternationalLicense.Columns[0].HeaderText = "Int.License ID";
                dgvListInternationalLicense.Columns[0].Width = 100;

                dgvListInternationalLicense.Columns[1].HeaderText = "Application ID";
                dgvListInternationalLicense.Columns[1].Width = 100;

                dgvListInternationalLicense.Columns[2].HeaderText = "Driver ID";
                dgvListInternationalLicense.Columns[2].Width = 100;

                dgvListInternationalLicense.Columns[3].HeaderText = "L.License ID";
                dgvListInternationalLicense.Columns[3].Width = 100;

                dgvListInternationalLicense.Columns[4].HeaderText = "Issue Date";
                dgvListInternationalLicense.Columns[4].Width = 200;

                dgvListInternationalLicense.Columns[5].HeaderText = "Expiration Date";
                dgvListInternationalLicense.Columns[5].Width = 200;

                dgvListInternationalLicense.Columns[6].HeaderText = "Is Active";
                dgvListInternationalLicense.Columns[6].Width = 140;

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = "";

            switch (cbFilterBy.Text)
            {
                case "International License ID":
                    ColumnFilter = "InternationalLicenseID";
                    break;
                case "Application ID":
                    ColumnFilter = "ApplicationID";
                    break;
                case "Driver ID":
                    ColumnFilter = "DriverID";
                    break;
                case "Local License ID":
                    ColumnFilter = "IssuedUsingLocalLicenseID";
                    break;
     
            }


            //Reset the filters in case nothing selected or filter value contain nothing
            if (txtFilterValue.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
                lblRecords.Text = _dtInternationalLicenseApplications.Rows.Count.ToString();
                return;
            }

       
            _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", ColumnFilter, txtFilterValue.Text);
          

            lblRecords.Text = _dtInternationalLicenseApplications.DefaultView.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
       {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "None")
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
                lblRecords.Text = _dtInternationalLicenseApplications.Rows.Count.ToString();
            }

            if (cbFilterBy.Text == "Is Active")
            {
                cbIsActive.Visible = true;
                txtFilterValue.Visible = false;
            }
            else
            {
                cbIsActive.Visible = false;
                
                txtFilterValue.Visible = (cbFilterBy.Text != "None");

                if (txtFilterValue.Visible)
                {
                    txtFilterValue.Text = "";
                    txtFilterValue.Focus();
                }
            }

        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = "";

            switch (cbIsActive.Text)
            {
                case "All":
                    FilterValue = "All";
                    break;
                case "Yes":
                    FilterValue = "True";
                    break;
                case "No":
                    FilterValue = "False";
                    break;
            }


            if (FilterValue == "All")
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
            else
                //in this case we deal with numbers not string.
                _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecords.Text = _dtInternationalLicenseApplications.DefaultView.Count.ToString();
        }

        private void personToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(clsLicense.Find((int)dgvListInternationalLicense.CurrentRow.Cells[3].Value).DriverInfo.PersonId);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo((int)dgvListInternationalLicense.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(clsLicense.Find((int)dgvListInternationalLicense.CurrentRow.Cells[3].Value).DriverInfo.PersonId);
            frm.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseApplication frm = new frmInternationalLicenseApplication();
            frm.ShowDialog();
        }
    }
}
