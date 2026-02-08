using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Domain.DTO
{
    public class clsInternationalLicenseDTO
    {
        public int InternationalLicenseID { get; set; }
        public int ApplicantID { get; set; }    
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }  
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }

        public clsInternationalLicenseDTO()
        {
            InternationalLicenseID = -1;
            ApplicantID = -1;
            DriverID = -1;
            IssuedUsingLocalLicenseID = -1;
            IssueDate = DateTime.MinValue;
            ExpirationDate = DateTime.MinValue;
            IsActive = true;
            CreatedByUserID = -1;
        }
    }
}
