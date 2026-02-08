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
    public partial class frmShowInternationalLicenseInfo : Form
    {
        private int _internationalLicenseID = -1;
        public frmShowInternationalLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();
            _internationalLicenseID = InternationalLicenseID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            ctrlInternationalLicenseInfo1.LoadInfo(_internationalLicenseID);
        }
    }
}
