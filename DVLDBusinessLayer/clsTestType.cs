using DVLDDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDBusinessLayer
{
    public class clsTestType
    {

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3}

        public clsTestType.enTestType TestTypeID { get; set; }
       // public int TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public Decimal TestTypeFees { get; set; }


        clsTestType (clsTestType.enTestType TestTypeID,string TestTypeTitle,string TestTypeDescription,Decimal TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = TestTypeTitle;
            this.TestTypeDescription = TestTypeDescription;
            this.TestTypeFees = TestTypeFees;
        }

        public static clsTestType Find (clsTestType.enTestType TestTypeID)
        {
            string TestTypeTitle = "", TestTypeDescription = "";
            Decimal TestTypeFees = 0;

            if (clsTestTypesData.GetTestTypeInfoByID((int)TestTypeID,ref TestTypeTitle,ref TestTypeDescription,ref TestTypeFees))
            {
                return new clsTestType(TestTypeID,TestTypeTitle,TestTypeDescription,TestTypeFees);
            }
            else
            {
                return null;
            }

        }

        public static DataTable GetAllTestType()
        {
            return clsTestTypesData.GetAllTestType();
        }

        public bool UpdateTestType()
        {
            return clsTestTypesData.UpdateTestType((int)this.TestTypeID,this.TestTypeTitle,this.TestTypeDescription,this.TestTypeFees);
        }


    }
}
