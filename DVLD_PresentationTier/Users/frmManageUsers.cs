using DVLDBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_PresentationTier
{
    public partial class frmManageUsers : Form
    {


        private static DataTable _dtAllUsers;

        private DataTable _dtUsers;
        public frmManageUsers()
        {
            InitializeComponent();
            _RefreshUsersList();
        }

        private void _RefreshUsersList()
        {

            _dtAllUsers = clsUser.GetAllUsers();
            _dtUsers = _dtAllUsers.DefaultView.ToTable(false, "UserID", "PersonID",
            "FullName", "UserName", "IsActive");

            dgvAllUsers.DataSource = _dtUsers;
            cbFilterBy.SelectedIndex = 0;

            lblRecords.Text = dgvAllUsers.Rows.Count.ToString();

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            

            if (dgvAllUsers.Rows.Count > 0)
            {
                dgvAllUsers.Columns[0].HeaderText = "User ID";
                dgvAllUsers.Columns[0].Width = 100;

                dgvAllUsers.Columns[1].HeaderText = "Person ID";
                dgvAllUsers.Columns[1].Width = 100;

                dgvAllUsers.Columns[2].HeaderText = "Full Name";
                dgvAllUsers.Columns[2].Width = 300;

                dgvAllUsers.Columns[3].HeaderText = "UserName";
                dgvAllUsers.Columns[3].Width = 100;

                dgvAllUsers.Columns[4].HeaderText = "Is Active";
                dgvAllUsers.Columns[4].Width = 100;


            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "None")
            {
                _RefreshUsersList();
            }

           

           

            if (cbFilterBy.Text == "Is Active")
            {
                // إخفاء التكست بوكس
                txtFilterValue.Visible = false;

                // إظهار الكومبو
                cbIsActiveValue.Visible = true;
                cbIsActiveValue.Focus();
                cbIsActiveValue.SelectedIndex = 0; // اختيار أول عنصر افتراضياً
            }
            else
            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");


                cbIsActiveValue.Visible = false;
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
                case "User ID":
                    ColumnFilter = "UserID";
                    break;
                case "Person ID":
                    ColumnFilter = "PersonID";
                    break;
                case "UserName":
                    ColumnFilter = "UserName";
                    break;
                case "Full Name":
                    ColumnFilter = "FullName";
                    break;
             


            }

            //Reset the filters in case nothing selected or filter value contain nothing

            if (txtFilterValue.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _dtUsers.DefaultView.RowFilter = "";
                lblRecords.Text = dgvAllUsers.Rows.Count.ToString();
                return;
            }

            if (ColumnFilter == "UserID" || ColumnFilter == "PersonID")
            {
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", ColumnFilter, txtFilterValue.Text);
            }
            else
            {
                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", ColumnFilter, txtFilterValue.Text);
            }

            lblRecords.Text = dgvAllUsers.Rows.Count.ToString();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
            if (txtFilterValue.Text == "")
            {
                _RefreshUsersList();
            }
        }

        private void cbIsActiveValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActiveValue.Text;
            switch (FilterValue)
            {
                case "All":
                    //_dtUsers.DefaultView.RowFilter = "IsActive = 'true' Or IsActive = 'false'";
                    break;
                case "Yes":
                    FilterValue = "1";
                    //_dtUsers.DefaultView.RowFilter = "IsActive = 'true'";
                    break;
                case "No":
                    FilterValue = "0";
                    //_dtUsers.DefaultView.RowFilter = "IsActive = 'false'";
                    break;

            }

            if (FilterValue == "All")
            {
                _dtUsers.DefaultView.RowFilter = "";
            }
            else
            {
                //in this case we deal with numbers not string

                _dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}",FilterColumn,FilterValue);

            }

            lblRecords.Text = dgvAllUsers.Rows.Count.ToString();


        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();
            frm.ShowDialog();
           // frmManageUsers_Load(null, null);
            _RefreshUsersList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshUsersList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete User [" + dgvAllUsers.CurrentRow.Cells[0].Value + "]"
                , "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsGlobal.CurrentUser.UserID != (int)dgvAllUsers.CurrentRow.Cells[0].Value) 
                {
                    if (clsUser.DeleteUser((int)dgvAllUsers.CurrentRow.Cells[0].Value))
                    {
                        MessageBox.Show("User Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _RefreshUsersList();
                    }
                    else
                    {
                        MessageBox.Show("Error: User is not Deleted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Cannot be deleted the current User","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo((int)dgvAllUsers.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshUsersList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
