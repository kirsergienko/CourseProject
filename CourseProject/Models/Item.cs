using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Item
    {
        public int Id { get; set; }

        public int CollectionId { get; set; }

        public DateTime LastChange { get; set; }
    }
}