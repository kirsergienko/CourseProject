namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class valuesCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Collections", "temsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Collections", "IntValuesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Collections", "BoolValuesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Collections", "StringValuesCount", c => c.Int(nullable: false));
            AddColumn("dbo.Collections", "DateValuesCount", c => c.DateTime(nullable: false));
            DropColumn("dbo.Collections", "ItemsCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Collections", "ItemsCount", c => c.Int(nullable: false));
            DropColumn("dbo.Collections", "DateValuesCount");
            DropColumn("dbo.Collections", "StringValuesCount");
            DropColumn("dbo.Collections", "BoolValuesCount");
            DropColumn("dbo.Collections", "IntValuesCount");
            DropColumn("dbo.Collections", "temsCount");
        }
    }
}
