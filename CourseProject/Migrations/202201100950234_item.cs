namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class item : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "LastChanged", c => c.DateTime(nullable: false));
            AddColumn("dbo.Items", "Tags", c => c.String());
            CreateIndex("dbo.BoolValues", "ItemId");
            CreateIndex("dbo.Items", "CollectionId");
            CreateIndex("dbo.Comments", "ItemId");
            CreateIndex("dbo.DateValues", "ItemId");
            CreateIndex("dbo.IntValues", "ItemId");
            CreateIndex("dbo.Likes", "ItemId");
            CreateIndex("dbo.StringValues", "ItemId");
            AddForeignKey("dbo.BoolValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Comments", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DateValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IntValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Likes", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StringValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Items", "CollectionId", "dbo.Collections", "Id", cascadeDelete: true);
            DropColumn("dbo.Items", "LastChange");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "LastChange", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Items", "CollectionId", "dbo.Collections");
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
            DropIndex("dbo.Items", new[] { "CollectionId" });
            DropIndex("dbo.BoolValues", new[] { "ItemId" });
            DropColumn("dbo.Items", "Tags");
            DropColumn("dbo.Items", "LastChanged");
        }
    }
}
