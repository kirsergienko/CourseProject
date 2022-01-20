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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
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
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Text = c.String(),
                        UserName = c.String(),
                        Commented = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.DateValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.IntValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastChanged = c.DateTime(nullable: false),
                        CollectionId = c.Int(nullable: false),
                        Tags = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.StringValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        Name = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
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
            DropForeignKey("dbo.StringValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Likes", "ItemId", "dbo.Items");
            DropForeignKey("dbo.IntValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.DateValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Comments", "ItemId", "dbo.Items");
            DropForeignKey("dbo.BoolValues", "ItemId", "dbo.Items");
            DropIndex("dbo.StringValues", new[] { "ItemId" });
            DropIndex("dbo.Likes", new[] { "ItemId" });
            DropIndex("dbo.IntValues", new[] { "ItemId" });
            DropIndex("dbo.DateValues", new[] { "ItemId" });
            DropIndex("dbo.Comments", new[] { "ItemId" });
            DropIndex("dbo.BoolValues", new[] { "ItemId" });
            DropTable("dbo.UserModels");
            DropTable("dbo.Tags");
            DropTable("dbo.StringValues");
            DropTable("dbo.Likes");
            DropTable("dbo.Items");
            DropTable("dbo.IntValues");
            DropTable("dbo.DateValues");
            DropTable("dbo.Comments");
            DropTable("dbo.Collections");
            DropTable("dbo.BoolValues");
        }
    }
}
