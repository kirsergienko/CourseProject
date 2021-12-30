using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseProject.Validators;

namespace CourseProject.Models
{
    public class Collection
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Theme { get; set; }

        public string Image { get; set; }

        [IsCount]
        public int ItemsCount { get; set; }

        public List<Item> Items { get; set; }
    }
}