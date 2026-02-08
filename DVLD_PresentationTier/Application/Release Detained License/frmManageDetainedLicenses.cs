using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLayer;

namespace DVLD_PresentationTier
{
    public partial class frmManageDetainedLicenses : Form
    {
        private DataTable _dtDetainedLicenses;

        public frmManageDetainedLicenses()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageDetainedLicenses_Load(object sender, EventArgs e)
        {
            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();

            dgvDetainedLicense.DataSource = _dtDetainedLicenses;
            cbFilterBy.SelectedIndex = 0;

            lblRecordsCount.Text = _dtDetainedLicenses.Rows.Count.ToString();

            if (_dtDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicense.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicense.Columns[0].Width = 80;

                dgvDetainedLicense.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicense.Columns[1].Width = 80;

                dgvDetainedLicense.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicense.Columns[2].Width = 120;

                dgvDetainedLicense.Columns[3].HeaderText = "Is Release";
                dgvDetainedLicense.Columns[3].Width = 80;

                dgvDetainedLicense.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicense.Columns[4].Width = 100;

                dgvDetainedLicense.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicense.Columns[5].Width = 120;

                dgvDetainedLicense.Columns[6].HeaderText = "N.No";
                dgvDetainedLicense.Columns[6].Width = 60;

                dgvDetainedLicense.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicense.Columns[7].Width = 200;

                dgvDetainedLicense.Columns[8].HeaderText = "Release.App.ID";
                dgvDetainedLicense.Columns[8].Width = 120;

            }

        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();

            frmManageDetainedLicenses_Load(null, null);
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            frmManageDetainedLicenses_Load(null, null);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Released")
            {
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }
            else
            {
                cbIsReleased.Visible = false;
                txtFilterValue.Visible = (cbFilterBy.Text != "None");

                if (txtFilterValue.Visible)
                {
                    txtFilterValue.Text = "";
                    txtFilterValue.Focus();
                }
            }

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = "";

            switch (cbFilterBy.Text)
            {
                case "Detain ID":
                    ColumnFilter = "DetainID";
                    break;
              
                case "National No":
                    ColumnFilter = "NationalNo";
                    break;
                case "Full Name":
                    ColumnFilter = "FullName";
                    break;
                case "Release Application ID":
                    ColumnFilter = "ReleaseApplicationID";
                    break;
                case "None":
                    ColumnFilter = "None";
                    break;
            }


            //Reset the filters in case nothing selected or filter value contain nothing
            if (txtFilterValue.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                lblRecordsCount.Text = _dtDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if (ColumnFilter == "DetainID" || ColumnFilter == "ReleaseApplicationID")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", ColumnFilter, txtFilterValue.Text);
            }
            else
            {
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", ColumnFilter, txtFilterValue.Text);
            }

            lblRecordsCount.Text = _dtDetainedLicenses.DefaultView.Count.ToString();
        }

        private void showPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(clsLicense.Find((int)dgvDetainedLicense.CurrentRow.Cells[1].Value).DriverInfo.PersonId);
            frm.ShowDialog();


        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLicenseInfo frm = new frmLicenseInfo((int)dgvDetainedLicense.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(clsLicense.Find((int)dgvDetainedLicense.CurrentRow.Cells[1].Value).DriverInfo.PersonId);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense((int)dgvDetainedLicense.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            frmManageDetainedLicenses_Load(null, null);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dgvDetainedLicense.CurrentRow.Cells[3].Value;
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsReleased";
            string FilterValue = "";

            switch(cbIsReleased.Text)
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
                _dtDetainedLicenses.DefaultView.RowFilter = "";
            else
                //in this case we deal with numbers not string.
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecordsCount.Text = _dtDetainedLicenses.DefaultView.Count.ToString();
        }
    }
}
