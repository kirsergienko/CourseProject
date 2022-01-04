namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "LastChange", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Items", "LastChange");
        }
    }
}
