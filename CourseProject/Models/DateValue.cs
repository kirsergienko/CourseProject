using System;
using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models
{
    public class DateValue
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Value { get; set; }
    }
}