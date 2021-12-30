using CourseProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseProject.Validators
{
    public class UniqueEmailAddres : ValidationAttribute
    {
        private ApplicationContext context = new ApplicationContext();

        public override bool IsValid(object Email)
        {
            return !context.Users.Any(x => x.EMail == (string)Email);
        }

        public override string FormatErrorMessage(string name)
        {
            return "This Email is already in use.";
        }
    }
}