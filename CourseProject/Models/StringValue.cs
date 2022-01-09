using System.Web.Mvc;

namespace CourseProject.Models
{
    public class StringValue
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public string Name { get; set; }

        [AllowHtml]
        public string Value { get; set; }
    }
}