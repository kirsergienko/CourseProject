namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoolValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Collections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Theme = c.String(),
                        Image = c.String(),
                        ItemsCount = c.Int(nullable: false),
                        IntValuesCount = c.Int(nullable: false),
                        BoolValuesCount = c.Int(nullable: false),
                        StringValuesCount = c.Int(nullable: false),
                        DateValuesCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DateValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IntValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CollectionId = c.Int(nullable: false),
                        LastChange = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StringValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        EMail = c.String(),
                        Password = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                        IsChecked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserModels");
            DropTable("dbo.Tags");
            DropTable("dbo.StringValues");
            DropTable("dbo.Items");
            DropTable("dbo.IntValues");
            DropTable("dbo.DateValues");
            DropTable("dbo.Collections");
            DropTable("dbo.BoolValues");
        }
    }
}
