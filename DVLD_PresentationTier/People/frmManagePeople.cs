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
    public partial class frmManagePeople : Form
    {
        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID","NationalNo",
            "FirstName", "secondName","ThirdName","LastName","DateOfBirth",
            "Gendor","Phone","CountryName","Email"
            );

   
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void _RefreshPeopleList()
        {
            _dtAllPeople = clsPerson.GetAllPeople(); // إعادة تحميل جميع الناس
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false,
                "PersonID", "NationalNo", "FirstName", "secondName", "ThirdName", "LastName",
                "DateOfBirth", "Gendor", "Phone", "CountryName", "Email");


            dgvAllPeople.DataSource = _dtPeople;
           
            lblRecordCount.Text = dgvAllPeople.Rows.Count.ToString();

        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            //فالواقع، الـ DataGridView راه ما كيخدمش بالـ DataTable نفسها، ولكن كياخد DefaultView ديالها تلقائيًا.
            dgvAllPeople.DataSource = _dtPeople;


            cbFilterBy.SelectedIndex = 0;


            lblRecordCount.Text = dgvAllPeople.Rows.Count.ToString();

            if (dgvAllPeople.Rows.Count > 0)
            {
                dgvAllPeople.Columns[0].HeaderText = "Person ID";
                dgvAllPeople.Columns[0].Width = 100;

                dgvAllPeople.Columns[1].HeaderText = "National No";
                dgvAllPeople.Columns[1].Width = 100;

                dgvAllPeople.Columns[2].HeaderText = "First Name";
                dgvAllPeople.Columns[2].Width = 100;

                dgvAllPeople.Columns[3].HeaderText = "Second Name";
                dgvAllPeople.Columns[3].Width = 100;

                dgvAllPeople.Columns[4].HeaderText = "Third Name";
                dgvAllPeople.Columns[4].Width = 100;

                dgvAllPeople.Columns[5].HeaderText = "Last Name";
                dgvAllPeople.Columns[5].Width = 100;

                dgvAllPeople.Columns[6].HeaderText = "Date Of Birth";
                dgvAllPeople.Columns[6].Width = 100;

                dgvAllPeople.Columns[7].HeaderText = "Gendor";
                dgvAllPeople.Columns[7].Width = 60;


                dgvAllPeople.Columns[8].HeaderText = "Phone";
                dgvAllPeople.Columns[8].Width = 80;


                dgvAllPeople.Columns[9].HeaderText = "Nationality";
                dgvAllPeople.Columns[9].Width = 80;

                dgvAllPeople.Columns[10].HeaderText = "Email";
                dgvAllPeople.Columns[10].Width = 80;

            }
            

           
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPeople();
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPeople((int)dgvAllPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvAllPeople.CurrentRow.Cells[0].Value + "]"
                , "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsPerson.DeletePerson((int)dgvAllPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully.","Deleted",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    _RefreshPeopleList();
                }
                else
                {
                    MessageBox.Show("Error : Person is not Deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPeople(-1);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmPersonDetails((int)dgvAllPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtFilterValue.Visible = (cbFilterBy.Text != "None");


            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
     
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string ColumnFilter = "";

            switch (cbFilterBy.Text)
            {


                case "Person ID":
                    ColumnFilter = "PersonID";

                    break;
                case "National No":
                    ColumnFilter = "NationalNo";
                    break;
                case "First Name":
                    ColumnFilter = "FirstName";
                    break;
                case "Second Name":
                    ColumnFilter = "SecondName";
                    break;
                case "Third Name":
                    ColumnFilter = "ThirdName";
                    break;
                case "Last Name":
                    ColumnFilter = "LastName";
                    break;
                case "Gendor":
                    ColumnFilter = "Gendor";
                    break;
                case "Nationality":
                    ColumnFilter = "CountryName";
                    break;
                case "Phone":
                    ColumnFilter = "Phone";
                    break;
                case "Email":
                    ColumnFilter = "Email";
                    break;


            }

            //Reset the filters in case nothing selected or filter value contain nothing
            if (txtFilterValue.Text.Trim() == "" || cbFilterBy.Text == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecordCount.Text = dgvAllPeople.Rows.Count.ToString();
                return;
            }

            if (ColumnFilter == "PersonID")
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", ColumnFilter, txtFilterValue.Text);

            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", ColumnFilter, txtFilterValue.Text);
              
            }


            lblRecordCount.Text = dgvAllPeople.Rows.Count.ToString();

            //dgvAllPeople.DataSource = _dtPeople.DefaultView;
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
            if (txtFilterValue.Text == "")
            {
                _RefreshPeopleList();
            }
        }

        private void dgvAllPeople_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
