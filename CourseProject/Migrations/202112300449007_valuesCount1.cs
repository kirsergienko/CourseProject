namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class valuesCount1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Collections", "ItemsCount", c => c.Int(nullable: false));
            DropColumn("dbo.Collections", "temsCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Collections", "temsCount", c => c.Int(nullable: false));
            DropColumn("dbo.Collections", "ItemsCount");
        }
    }
}
