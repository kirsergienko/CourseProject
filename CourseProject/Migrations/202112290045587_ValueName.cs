namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValueName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoolValues", "Name", c => c.String());
            AddColumn("dbo.DateValues", "Name", c => c.String());
            AddColumn("dbo.IntValues", "Name", c => c.String());
            AddColumn("dbo.StringValues", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StringValues", "Name");
            DropColumn("dbo.IntValues", "Name");
            DropColumn("dbo.DateValues", "Name");
            DropColumn("dbo.BoolValues", "Name");
        }
    }
}
