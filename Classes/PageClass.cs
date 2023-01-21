using OhLordAgain.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OhLordAgain.Classes
{
    public static class PageClass
    {
        private static Authorization authorization;
        private static AdminPage adminPage;
        private static UserPage userPage;

        public static Authorization GetAuthorization()
        {
            if (authorization == null)
            {
                authorization = new Authorization();
            }
            return authorization;
        }

        public static AdminPage GetAdmin()
        {
            if (adminPage == null)
            {
                adminPage = new AdminPage();
            }
            return adminPage;
        }

        public static UserPage GetUser()
        {
            if (userPage == null)
            {
                userPage = new UserPage();
            }
            return userPage;
        }
    }
}
