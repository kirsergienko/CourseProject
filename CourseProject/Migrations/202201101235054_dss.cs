namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dss : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BoolValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Comments", "ItemId", "dbo.Items");
            DropForeignKey("dbo.DateValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.IntValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Likes", "ItemId", "dbo.Items");
            DropForeignKey("dbo.StringValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "CollectionId", "dbo.Collections");
            DropIndex("dbo.BoolValues", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "CollectionId" });
            DropIndex("dbo.Comments", new[] { "ItemId" });
            DropIndex("dbo.DateValues", new[] { "ItemId" });
            DropIndex("dbo.IntValues", new[] { "ItemId" });
            DropIndex("dbo.Likes", new[] { "ItemId" });
            DropIndex("dbo.StringValues", new[] { "ItemId" });
            AddColumn("dbo.Items", "LastChange", c => c.DateTime(nullable: false));
            DropColumn("dbo.Items", "LastChanged");
            DropColumn("dbo.Items", "Tags");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "Tags", c => c.String());
            AddColumn("dbo.Items", "LastChanged", c => c.DateTime(nullable: false));
            DropColumn("dbo.Items", "LastChange");
            CreateIndex("dbo.StringValues", "ItemId");
            CreateIndex("dbo.Likes", "ItemId");
            CreateIndex("dbo.IntValues", "ItemId");
            CreateIndex("dbo.DateValues", "ItemId");
            CreateIndex("dbo.Comments", "ItemId");
            CreateIndex("dbo.Items", "CollectionId");
            CreateIndex("dbo.BoolValues", "ItemId");
            AddForeignKey("dbo.Items", "CollectionId", "dbo.Collections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StringValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Likes", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IntValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DateValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BoolValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
        }
    }
}
