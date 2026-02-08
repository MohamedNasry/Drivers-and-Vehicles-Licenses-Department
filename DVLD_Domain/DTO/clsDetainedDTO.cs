using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Domain.DTO
{
    public class clsDetainedDTO
    {
        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public double FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }


        public clsDetainedDTO()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.MaxValue;
            FineFees = 0.0;

            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.MaxValue;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;

        }

    }
}
