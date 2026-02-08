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
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID;
        private clsDrivers _Driver;
        private DataTable _dtLocalDriverLicenseHistory;
        private DataTable _dtInternationalDriverLicenseHistory;




        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }


        private void _LoadLocalLicenseInfo()
        {
            _dtLocalDriverLicenseHistory = clsDrivers.GetLicenses(_DriverID);

            dgvLocalLicensesHistory.DataSource = _dtLocalDriverLicenseHistory;
            lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();

            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 60;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 60;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 260;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 150;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 150;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 80;

            }
        }


        private void _LoadInternationalLicenseInfo()
        {

            _dtInternationalDriverLicenseHistory = clsDrivers.GetInternationalLicenses(_DriverID);


            dgvInternationalLicensesHistory.DataSource = _dtInternationalDriverLicenseHistory;
            lblInternationalRecordsCount.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();

            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 100;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 80;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 80;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 140;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 140;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 80;

            }
        }

        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDrivers.FindByDriverID(_DriverID);
            
            if (_Driver == null)
            {
                MessageBox.Show("There is no driver with id = " + DriverID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();
        }

        public void LoadInfoByPersonID(int PersonID)
        {
            _Driver = clsDrivers.FindByPersonID(PersonID);
            if (_Driver == null)
            {
                MessageBox.Show("There is no driver with person id = " + PersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID = _Driver.DriverId;
            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();
        }

        private void ctrlDriverLicenses_Load(object sender, EventArgs e)
        {

        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value;
            frmLicenseInfo frm = new frmLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        public void Clear()
        {
            _dtLocalDriverLicenseHistory.Clear();
            _dtInternationalDriverLicenseHistory.Clear();
        }
    }
}
