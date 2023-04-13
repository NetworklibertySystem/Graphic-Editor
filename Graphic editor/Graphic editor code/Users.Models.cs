using System;
using System.Collections.Generic;
using System.Linq;
namespace Graphic_editor
{
   public class User
    {
        public User()
        { }
      public  User(string login, string slogin, string password )
        {
            Login = login;
            Slogin = slogin;
            Password = password;
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Slogin { get; set; }
        public string Password { get; set; }
    }
    static class UserArray
    {
        public static List<User> users = new List<User>()
        {
            new User{Login = "creator", Slogin = "@creator", Password = "12345"},
            new User{Login = "Ilya", Slogin = "@Ilya", Password="20231"},
            new User{Login = "Mikhail", Slogin = "@Mikhail", Password="20002"},
            new User{Login = "Nikita", Slogin = "@Nikita", Password="20043"},
            new User{Login = "Demonolog", Slogin = "@Demonolog", Password="34312"},
            new User{Login = "Hawk", Slogin = "@Hawk", Password="56781"},
            new User{Login = "Henker", Slogin = "@Henker", Password = "23415"},
            new User{Login = "Vlad", Slogin = "@Vlad", Password = "23321"},
            new User{Login = "Vistan", Slogin = "@Vistan", Password = "34562"},
            new User{Login = "Deikun", Slogin = "@Deikun", Password = "43515"},
            new User{Login = "Bruno", Slogin = "@Bruno", Password = "34517"},
            new User{Login = "Reaper", Slogin = "@Reaper", Password = "32462"},
            new User{Login = "Fink", Slogin = "@Fink", Password = "78348"},
            new User{Login = "Scull", Slogin = "@Scull", Password = "56739"},
            new User{Login = "Kirill", Slogin = "@Kirill", Password = "67821"},
            new User{Login = "Winner", Slogin = "@Winner", Password = "45567"},
            new User{Login = "Zett", Slogin = "@Zett", Password = "43523"},
            new User{Login = "Carp", Slogin = "@Carp", Password = "56432"},
            new User{Login = "Matvey", Slogin = "@Matvey", Password = "54566"},
            new User{Login = "Extra", Slogin = "@Extra", Password = "43435"},
            new User{Login = "Gulaev", Slogin = "@Gulaev", Password = "54545"},
        };
        public static void AddUser(User user)
        {
            users.Add(user);
        }
    }
    public class MyUsername
    {
        private List<Usersname> MyUsersname {get{return MyUsersname;}set{MyUsersname = value;}}
    }
    class Usersname
    {
        public int ID {get {return ID;} private set {ID = value;}}
        public string Username {get {return Username;} private set {Username = value;}}
        public string Gender {get {return Gender;} private set {Gender = value;}}
    }
}
