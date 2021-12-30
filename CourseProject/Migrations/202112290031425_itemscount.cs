namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itemscount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Collections", "ItemsCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Collections", "ItemsCount");
        }
    }
}
