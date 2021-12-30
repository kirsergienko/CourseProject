namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class k : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Collections", "DateValuesCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Collections", "DateValuesCount", c => c.DateTime(nullable: false));
        }
    }
}
