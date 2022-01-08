using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public string Text { get; set; }

        public string UserName { get; set; }

        public DateTime Commented { get; set; }
    }
}