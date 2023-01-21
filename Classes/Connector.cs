using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhLordAgain.Classes
{
    public static class Connector
    {
        private static Database.user10Entities DatabaseConnector;
        private static Database.User UserData;

        public static Database.user10Entities GetDatabase()
        {
            if (DatabaseConnector == null)
            {
                DatabaseConnector = new Database.user10Entities();
            }
            return DatabaseConnector;
        }

        public static Database.User GetUserData()
        {
            return UserData;
        }

        public static bool Authorize(string login, string password)
        {
            string getlogin = login.Trim();
            string getpassword = password.Trim();
            UserData = GetDatabase().User.Where(us => us.UserLogin == getlogin && us.UserPassword == getpassword).FirstOrDefault();
            return UserData != null;
        }
    }
}
