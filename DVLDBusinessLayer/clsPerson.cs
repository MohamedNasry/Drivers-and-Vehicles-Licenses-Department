using System;
using System.Data;
using DVLDDataAccessLayer;

namespace DVLDBusinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1};
        
        public enMode Mode = enMode.AddNew;

        public int PersonID {  get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName + " " + ThirdName + " "+ LastName;
            }
        }
        public DateTime DateOfBirth { get; set; }
        public byte Gendor {  get; set; }
        public string Address {  get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }

        public clsCountry CountryInfo;

        private string _ImagePath;
        public string ImagePath 
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
            }
        }

        public clsPerson ()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 0;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;

        }

        private clsPerson(int PersonID,string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName
            , DateTime DateOfBirth,byte Gendor, string Address,string Phone, string Email,int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;

            this.CountryInfo = clsCountry.Find(NationalityCountryID);

            Mode = enMode.Update;
        }

        

        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "";
            string Email = "", ImagePath = "";
            int  NationalityCountryID = -1;
            byte Gendor = 0;
            DateTime DateOfBirth = DateTime.Now;

            if (clsPersonData.GetPersonInfoByID(PersonID,ref NationalNo,ref FirstName,ref SecondName,ref ThirdName,
                ref LastName,ref DateOfBirth,ref Gendor,ref Address,ref Phone,ref Email,ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo,FirstName,SecondName,ThirdName,LastName,DateOfBirth,Gendor,Address,Phone,Email,
                    NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
     
        }

        public static clsPerson Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "";
            string Email = "", ImagePath = "";
            int NationalityCountryID = -1, PersonID = 0;
            byte Gendor = 0;
            DateTime DateOfBirth = DateTime.Now;

            if (clsPersonData.GetPersonInfoByNationalNo(ref PersonID, NationalNo, ref FirstName, ref SecondName, ref ThirdName,
                ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email,
                    NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }

        }


        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
              return clsPersonData.UpdatePerson(this.PersonID, this.NationalNo, this.FirstName,this.SecondName,this.ThirdName, this.LastName,
                  this.DateOfBirth,this.Gendor, this.Address,this.Phone, this.Email,this.NationalityCountryID, this.ImagePath);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePerson();


            }
            return false;
        }

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }

        public static bool IsPersonExist(int ID)
        {
            return clsPersonData.IsPersonExist(ID);
        }

        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }


    }
}
