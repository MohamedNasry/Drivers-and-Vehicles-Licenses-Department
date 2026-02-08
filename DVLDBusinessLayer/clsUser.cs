using DVLDDataAccessLayer;
using System;
using System.Data;

namespace DVLDBusinessLayer
{
    public class clsUser
    {
        public enum enMode { AddNew = 0, Update = 1};

        public enMode Mode = enMode.AddNew;

        public int UserID {  get; set; }
        public int PersonID { get; set; }
        public string UserName {  get; set; }

        private string _Password;
        public string Password 
        { 
            get
            {
                return _Password; 
            }
            set 
            {
                _Password = value;
            } 
        }

        public clsPerson PersonInfo;
        public bool IsActive {  get; set; }

        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;

            Mode = enMode.AddNew;
        }

        public clsUser(int UserID,int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            this.PersonInfo = clsPerson.Find(PersonID);

            Mode = enMode.Update;
        }
    
        public static clsUser FindByUserID (int UserID)
        {
            string UserName = "", Password = "";
            int PersonID = -1;
            bool IsActive = false;

            if (clsUsersData.GetUserInfoByUserID(UserID, ref PersonID, ref UserName, 
                ref Password, ref IsActive))
            {
                return new clsUser(UserID,PersonID, UserName, Password, IsActive);
            }
            else
            {
                return null;
            }

        }

        public static clsUser FindByPersonID(int PersonID)
        {
            string UserName = "", Password = "";
            int UserID = -1;
            bool IsActive = false;

            bool IsFound = false;

            IsFound = clsUsersData.GetUserInfoByPersonID(PersonID, ref UserID, ref UserName,
                ref Password, ref IsActive);

            if (IsActive)
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
            {
                return null;
            }
        }


        public static clsUser FindByUserNameAndPassword (string UserName, string Password)
        {
     
            int PersonID = -1, UserID = -1;
            bool IsActive = false;

            bool IsFound = false;

            IsFound = clsUsersData.GetUserInfoByUserNameAndPassword(UserName, Password, ref
                UserID, ref PersonID, ref IsActive);

            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
            {
                return null;
            }
        }
    
        private bool _AddNewUser()
        {
            this.UserID = clsUsersData.AddNewUser(this.PersonID, this.UserName, this.Password,
                this.IsActive);

            return UserID != -1;
        }

        private bool _UpdateUser()
        {
            return clsUsersData.UpdateUser(this.UserID,this.PersonID,this.UserName,
                this.Password,this.IsActive);

        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateUser();
            }

            return false;
        }

        public static bool DeleteUser(int UserID)
        {
            return clsUsersData.DeleteUser(UserID);
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUsersData.IsUserExist(UserID);
        }

        public static bool IsUserExist(string UserName)
        {
            return clsUsersData.IsUserExist(UserName);
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUsersData.IsUserExistForPersonID(PersonID);
        }

        public static DataTable GetAllUsers()
        {
            return clsUsersData.GetAllUsers();
        }

        public static bool ChangePassword(int UserID,string NewPassword)
        {
            return clsUsersData.ChangePassword(UserID, NewPassword);
        }


    }
}
