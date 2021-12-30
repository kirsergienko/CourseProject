using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Item
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<Tag> Tags { get; set; }

        public List<IntValue> IntValues { get; set; }

        public List<StringValue> StringValues { get; set; }

        public List<BoolValue> BoolValues { get; set; }

        public List<DateValue> DateValues { get; set; }

    }
}