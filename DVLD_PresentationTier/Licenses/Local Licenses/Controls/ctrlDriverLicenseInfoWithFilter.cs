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
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {

        //Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;

        //create a protected method to raise the event with a parameter

        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(LicenseID);//Raise the event with the parameter
            }
        }
        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        public void FilterFocus()
        {
             txtFilterValue.Focus();    
        }

        private int _LicenseID = -1;

        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; }
        }

        public void LoadLicenseInfo(int LicenseID)
        {
            ctrlDriverLicenseInfo1.LoadInfo(LicenseID);
            _LicenseID = LicenseID;
            txtFilterValue.Text = LicenseID.ToString();

            if (OnLicenseSelected != null && FilterEnabled)
                //raise the event with the parameter
                LicenseSelected(_LicenseID);

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                //(char)13 → هو مفتاح Enter(لأن الكود ASCII ديالو = 13).

                //الشرط كيقول: إذا المستخدم ضغط على Enter → دير btnSearch.PerformClick();.

                //🔸 btnSearch.PerformClick(); = بحال إيلا المستخدم كليكة على زر Search بالماوس.
                //يعني كيعطيك اختصار: المستخدم يقدر يكتب قيمة → يضغط Enter → يبحث مباشرة.
                btnSearch.PerformClick();
            }
            //e.Handled = true; → تمنع الإدخال.
            e.Handled = !Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //here we dont continue because the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon to show error");
                return;
            }
            _FindNow();
        }

        private void _FindNow()
        {
            if (!int.TryParse(txtFilterValue.Text, out int parsedId))
            {
                MessageBox.Show("Please enter a valid numeric License ID.", "Invalid input",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFilterValue.Focus();
                return;
            }

            _LicenseID = parsedId;
            LoadLicenseInfo(_LicenseID);

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFilterValue.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, "this field is required!");

            }
            else
            {
                errorProvider1.SetError(txtFilterValue, null);
            }
        }
    }
}
