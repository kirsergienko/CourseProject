namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class item : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Items", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Title", c => c.String());
        }
    }
}
