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
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        //Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelected;

        //create a protected method to raise the event with a parameter

        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if (handler != null)
            {
                handler(PersonID);//Raise the event with the parameter
            }
        }

        private bool _ShowAddPerson = true;

        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }

            set
            {
                _ShowAddPerson=value;
                btnAddNewPerson.Visible = _ShowAddPerson;

            }
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
                _FilterEnabled=value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }


        
        
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }


        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; } 

        }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonCard1.SelectedPersonInfo; }
        }

        //public void LoadPersonInfo(int PersonID)
        //{
        //    cbFilterBy.SelectedIndex = 1;
        //    txtFilterValue.Text = PersonID.ToString();
        //    _FindNow();
        //}

        public void LoadPersonInfo( int PersonID )
        {
            ctrlPersonCard1.LoadPersonInfo( PersonID );
            txtFilterValue.Visible = true;
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            
           // _FindNow();

        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }

        private void _FindNow()
        {
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));
                    break;
                case "National No":
                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;
            }

            if (OnPersonSelected != null && FilterEnabled)
            {
                //Raise the event with a parameter
                //OnPersonSelected(ctrlPersonCard1.PersonID);
                PersonSelected(ctrlPersonCard1.PersonID);
            }
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

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPeople frm = new frmAddEditPeople();
            frm.DataBack += _DataBackEvent;//Subscribe to the event
            frm.ShowDialog();
        }

        private void _DataBackEvent(object sender,int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            ctrlPersonCard1.LoadPersonInfo(PersonID);
            
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //كيتنفذ مباشرة ملي المستخدم يضغط على مفتاح (key) في لوحة المفاتيح.
            //Check if the pressed key is enter (character code 13)

            if (e.KeyChar == (char)13)
            {
                //(char)13 → هو مفتاح Enter(لأن الكود ASCII ديالو = 13).

                //الشرط كيقول: إذا المستخدم ضغط على Enter → دير btnSearch.PerformClick();.

                //🔸 btnSearch.PerformClick(); = بحال إيلا المستخدم كليكة على زر Search بالماوس.
                //يعني كيعطيك اختصار: المستخدم يقدر يكتب قيمة → يضغط Enter → يبحث مباشرة.
                btnSearch.PerformClick();
            }

            //this will allow only digits if person id is selected

            if (cbFilterBy.Text == "Person ID")
            {

                    // Char.IsDigit(e.KeyChar) → صحيح إذا الحرف المدخل رقم(0 - 9).

                    //char.IsControl(e.KeyChar) → صحيح إذا الزر خاص بالتحكم(بحال Backspace, Delete...).

                    //!Char.IsDigit(...) && !char.IsControl(...) → يعني المستخدم كتب حاجة ماشي رقم وماشي زر تحكم.

                    //e.Handled = true; → تمنع الإدخال.
                e.Handled = !Char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            }
        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {

        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if (FilterEnabled) 
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
}
