using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Services
{
    public class LoginManager
    {
        private ApplicationContext context = new ApplicationContext();

        public bool IsRegistred(string login, string password)
        {
            if (context.Users.Any(x => x.UserName == login && x.Password == password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsRegistred(UserModel user)
        {
            if (context.Users.Any(x => x.UserName == user.UserName && x.Password == user.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsBlocked(UserModel user)
        {
            var u = context.Users.FirstOrDefault(x => x.UserName == user.UserName);
            return u.IsBlocked;
        }
    }
}