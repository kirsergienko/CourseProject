using CourseProject.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CourseProject.Validators
{
    public class UniqueTitle : ValidationAttribute
    {
        private ApplicationContext context = new ApplicationContext();

        public UniqueTitle()
        {
            AllowEmptyStrings = false;
        }

        public bool AllowEmptyStrings { get; set; }

        public override bool IsValid(object title)
        {
            return !context.Collections.Any(x => x.Title == (string)title);
        }

        public override string FormatErrorMessage(string name)
        {
            return "This name already exists!";
        }
    }
}