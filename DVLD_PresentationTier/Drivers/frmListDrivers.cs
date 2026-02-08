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
    public partial class frmListDrivers : Form
    {

        private DataTable _dtDrivers;
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _dtDrivers = clsDrivers.GetAll();
            dgvListDrivers.DataSource = _dtDrivers;

            cbFilterBy.SelectedIndex = 0;

            lblRecordCount.Text = _dtDrivers.Rows.Count.ToString();

            if (dgvListDrivers.Rows.Count > 0)
            {
                dgvListDrivers.Columns[0].HeaderText = "Driver ID";
                dgvListDrivers.Columns[0].Width = 100;

                dgvListDrivers.Columns[1].HeaderText = "Person ID";
                dgvListDrivers.Columns[1].Width = 100;

                dgvListDrivers.Columns[2].HeaderText = "National No";
                dgvListDrivers.Columns[2].Width = 100;

                dgvListDrivers.Columns[3].HeaderText = "Full Name";
                dgvListDrivers.Columns[3].Width = 300;

                dgvListDrivers.Columns[4].HeaderText = "Date";
                dgvListDrivers.Columns[4].Width = 150;

                dgvListDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvListDrivers.Columns[5].Width = 200;


            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.Text != "None";

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = "";

            switch (cbFilterBy.Text)
            {
                case "Driver ID":
                    ColumnFilter = "DriverID";
                    break;
                case "Person ID":
                    ColumnFilter = "PersonID";
                    break;
                case "National No":
                    ColumnFilter = "NationalNo";
                    break;
                case "Full Name":
                    ColumnFilter = "FullName";
                    break;
              
            }


            //Reset the filters in case nothing selected or filter value contain nothing
            if (txtFilterValue.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _dtDrivers.DefaultView.RowFilter = "";
                lblRecordCount.Text = _dtDrivers.Rows.Count.ToString();
                return;
            }

            if (ColumnFilter == "DriverID" || ColumnFilter == "PersonID")
            {
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", ColumnFilter, txtFilterValue.Text);
            }
            else
            {
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", ColumnFilter, txtFilterValue.Text);
            }

            lblRecordCount.Text = _dtDrivers.DefaultView.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails((int)dgvListDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory((int)dgvListDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }
    }
}
