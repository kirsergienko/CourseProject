using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CourseProject.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("myDb")
        {

        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserModel> Users { get; set; }

        public DbSet<Collection> Collections { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<BoolValue> BoolValues { get; set; }

        public DbSet<IntValue> IntValues { get; set; }

        public DbSet<StringValue> StringValues { get; set; }

        public DbSet<DateValue> DateValues { get; set; }
    }
}