using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Domain.DAO
{
    public class clsTestDTO
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public byte TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public clsTestDTO()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = 0;
            Notes = string.Empty;
            CreatedByUserID = -1;
        }


    }
}
