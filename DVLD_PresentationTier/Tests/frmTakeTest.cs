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
    public partial class frmTakeTest : Form
    {
        private int _TestAppointmentID = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        private int _TestID = -1;
        private clsTest _Test;
        public frmTakeTest(int TestAppointmentID , clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestAppointmentID = TestAppointmentID;
            _TestTypeID = TestTypeID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestTypeID = _TestTypeID;
            ctrlScheduledTest1.LoadInfo(_TestAppointmentID);

            if (ctrlScheduledTest1.TestAppointmentID == -1)
            {
                btnSave.Enabled = false;
            }
            else 
                btnSave.Enabled = true;

            _TestID = ctrlScheduledTest1.TestID;
            if(_TestID != -1)
            {
                _Test = clsTest.Find(_TestID);
                if(_Test != null)
                {
                    if(_Test.TestResult == 1)
                    {
                        rbPass.Checked = true;
                    }
                    else
                    {
                        rbFail.Checked = true;
                    }
                    txtNotes.Text = _Test.Notes;

                    lblUserMessage.Visible = true;
                    rbFail.Enabled = false;
                    rbPass.Enabled = false;
                }
            }
            else
            {
                lblUserMessage.Visible = false;
                _Test = new clsTest();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            _Test.TestAppointmentID = _TestAppointmentID;
            if(rbPass.Checked)
            {
                _Test.TestResult = 1;
            }
            else
            {
                _Test.TestResult = 0;
            }

            _Test.Notes = txtNotes.Text;
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(_Test.Save())
            {
                MessageBox.Show("Test results saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed to save test results.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
