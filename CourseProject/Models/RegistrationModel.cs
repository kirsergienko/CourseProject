using CourseProject.Validators;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class RegistrationModel
    {
        [Required]
        [UniqueName]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [UniqueEmailAddres]
        public string EMail { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirmation { get; set; }

    }
}