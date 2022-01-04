using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class AddItemModel
    {
        public int Id { get; set; }

        public DateTime LastChanged { get; set; }

        public int CollectionId { get; set; }

        public string Tags { get; set; }

        public List<IntValue> IntValues { get; set; }

        public List<BoolValue> BoolValues { get; set; }

        public List<StringValue> StringValues { get; set; }

        public List<DateValue> DateValues { get; set; }
    }
}