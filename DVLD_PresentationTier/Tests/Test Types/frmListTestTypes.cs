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
    public partial class frmListTestTypes : Form
    {
        private DataTable _dtAllTestTypes;
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _dtAllTestTypes = clsTestType.GetAllTestType();

            dgvTestTypes.DataSource = _dtAllTestTypes;

            lblCountRecords.Text = dgvTestTypes.Rows.Count.ToString();

            if (dgvTestTypes.Rows.Count > 0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 50;

                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 100;

                dgvTestTypes.Columns[2].HeaderText = "Description";
                dgvTestTypes.Columns[2].Width = 400;

                dgvTestTypes.Columns[3].HeaderText = "Fees";
                dgvTestTypes.Columns[3].Width = 80;

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTestType frm = new frmUpdateTestType((clsTestType.enTestType)dgvTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListTestTypes_Load(null, null);

        }
    }
}
