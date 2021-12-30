namespace CourseProject.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string EMail { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsBlocked { get; set; }

        public bool IsChecked { get; set; }
    }
}