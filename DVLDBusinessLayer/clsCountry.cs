using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLDDataAccessLayer;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DVLDBusinessLayer
{
    public class clsCountry
    {
        public enum enMode { AddNew = 0, Update = 1 };

        public enMode Mode = enMode.AddNew;

        public int CountryID {  get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            CountryID = -1;
            CountryName = "";

            Mode = enMode.AddNew;
        }

        private clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;

            Mode = enMode.Update;
        }

        private bool _AddNewCountry()
        {
            this.CountryID = clsCountryData.AddNewCountry(CountryName);

            return (this.CountryID != -1);
        }

        private bool _UpdateCountry()
        {
            return clsCountryData.UpdateCountry(this.CountryID, this.CountryName);
        }

        public static clsCountry Find(int  CountryID)
        {
            string CountryName = "";

            if (clsCountryData.GetCountryInfoByID(CountryID,ref CountryName))
            {
                return new clsCountry(CountryID,CountryName);
            }
            else
            {
                return null;
            }
        }

        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;

            if (clsCountryData.GetCountryInfoByName(ref CountryID, CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }

        bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewCountry())
                    {
                        Mode = enMode.Update;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:

                    return _UpdateCountry();

            }
            return false;
        }


        public static bool IsCountryExist(int CountryID)
        {
            return clsCountryData.IsCountryExist(CountryID);
        }

        public static bool IsCountryExist(string CountryName)
        {
            return clsCountryData.IsCountryExist(CountryName);
        }


    }
}
