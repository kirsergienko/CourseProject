using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class Item
    {
        public int Id { get; set; }

        public DateTime LastChanged { get; set; }

        public int CollectionId { get; set; }

        public string Tags { get; set; }

        public virtual List<IntValue> IntValues { get; set; }

        public virtual List<BoolValue> BoolValues { get; set; }

        public virtual List<StringValue> StringValues { get; set; }

        public virtual List<DateValue> DateValues { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Like> Likes { get; set; }
    }
}