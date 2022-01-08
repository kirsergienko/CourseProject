namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commented : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Commented", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Commented");
        }
    }
}
