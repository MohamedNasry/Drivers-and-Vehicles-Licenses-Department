using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Domain.DTO
{
    public class clsDriverDTO
    {

        public int DriverId { get; set; }
        public int PersonId { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }


        public clsDriverDTO()
        {
            this.CreatedByUserID = -1;
            DriverId = -1;
            PersonId = -1;
            CreatedDate = DateTime.Now;
        }

    }
}
