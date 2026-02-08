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
    public partial class frmListApplicationTypes : Form
    {
        private DataTable _dtAllApplicationTypes;
        public frmListApplicationTypes()
        {
            InitializeComponent();
        }

      

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationType();

            dgvAplicationTypes.DataSource = _dtAllApplicationTypes;

            lblRecordsCount.Text = dgvAplicationTypes.Rows.Count.ToString();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateApplicationType frm = new frmUpdateApplicationType((int)dgvAplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmManageApplicationTypes_Load(null, null); 
        }
    }
}
