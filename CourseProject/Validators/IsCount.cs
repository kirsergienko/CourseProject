using System.ComponentModel.DataAnnotations;

namespace CourseProject.Validators
{
    public class IsCount : ValidationAttribute
    {
        public IsCount()
        {
            AllowEmptyStrings = false;
        }

        public bool AllowEmptyStrings { get; set; }

        public override bool IsValid(object input)
        {
            return int.TryParse((string)input.ToString(), out int _);
        }

        public override string FormatErrorMessage(string name)
        {
            return "This must be a number.";
        }
    }
}