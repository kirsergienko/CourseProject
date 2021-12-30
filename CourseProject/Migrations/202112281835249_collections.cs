namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class collections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoolValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Collection_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Collections", t => t.Collection_Id)
                .Index(t => t.Collection_Id);
            
            CreateTable(
                "dbo.DateValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
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
                        Value = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Items", t => t.ItemId, cascadeDelete: true)
                .Index(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Collection_Id", "dbo.Collections");
            DropForeignKey("dbo.Tags", "ItemId", "dbo.Items");
            DropForeignKey("dbo.StringValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.IntValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.DateValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.BoolValues", "ItemId", "dbo.Items");
            DropIndex("dbo.Tags", new[] { "ItemId" });
            DropIndex("dbo.StringValues", new[] { "ItemId" });
            DropIndex("dbo.IntValues", new[] { "ItemId" });
            DropIndex("dbo.DateValues", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "Collection_Id" });
            DropIndex("dbo.BoolValues", new[] { "ItemId" });
            DropTable("dbo.Tags");
            DropTable("dbo.StringValues");
            DropTable("dbo.IntValues");
            DropTable("dbo.DateValues");
            DropTable("dbo.Items");
            DropTable("dbo.Collections");
            DropTable("dbo.BoolValues");
        }
    }
}
