using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsApplicationType
    {

        public int ApplicationTypeID {  get; set; }
        public string ApplicationTypeTitle {  get; set; }
        public Decimal ApplicationFees { get; set; }

        clsApplicationType(int ApplicationTypeID,string ApplicationTypeTitle,Decimal ApplicationFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationFees = ApplicationFees;
        }

        public static clsApplicationType Find (int  ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";
            Decimal ApplicationFees = 0;

            if (clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID,ref ApplicationTypeTitle,
                ref ApplicationFees))
            {
                return new clsApplicationType(ApplicationTypeID,ApplicationTypeTitle,ApplicationFees);
            }
            else
            {
                return null;
            }
        }


        public static DataTable GetAllApplicationType()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();
        }

        public bool UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationTypes(this.ApplicationTypeID,this.ApplicationTypeTitle
                ,this.ApplicationFees);
        }

    }
}
