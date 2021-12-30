using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CourseProject.Validators;

namespace CourseProject.Models
{
    public class Collection
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [UniqueTitle]
        [MinLength(3)]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Theme { get; set; }

        public string Image { get; set; }

        public int ItemsCount { get; set; }

        [IsCount]
        public int IntValuesCount { get; set; }

        [IsCount]
        public int BoolValuesCount { get; set; }

        [IsCount]
        public int StringValuesCount { get; set; }

        [IsCount]
        public int DateValuesCount { get; set; }
    }
}