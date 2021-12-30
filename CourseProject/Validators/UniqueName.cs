using CourseProject.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CourseProject.Validators
{
    public class UniqueName : ValidationAttribute
    {
        private ApplicationContext context = new ApplicationContext();

        public UniqueName()
        {
            AllowEmptyStrings = false;
        }

        public bool AllowEmptyStrings { get; set; }

        public override bool IsValid(object userName)
        {
            return !context.Users.Any(x => x.UserName == (string)userName);
        }

        public override string FormatErrorMessage(string name)
        {
            return "This name already exists!";
        }
    }
}