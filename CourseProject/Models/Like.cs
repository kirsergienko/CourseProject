using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Like
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int UserId { get; set; }
    }
}