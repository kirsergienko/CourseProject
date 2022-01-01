namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class s : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BoolValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.DateValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.IntValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.StringValues", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Tags", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Items", "CollectionId", "dbo.Collections");
            DropIndex("dbo.BoolValues", new[] { "ItemId" });
            DropIndex("dbo.Items", new[] { "CollectionId" });
            DropIndex("dbo.DateValues", new[] { "ItemId" });
            DropIndex("dbo.IntValues", new[] { "ItemId" });
            DropIndex("dbo.StringValues", new[] { "ItemId" });
            DropIndex("dbo.Tags", new[] { "ItemId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Tags", "ItemId");
            CreateIndex("dbo.StringValues", "ItemId");
            CreateIndex("dbo.IntValues", "ItemId");
            CreateIndex("dbo.DateValues", "ItemId");
            CreateIndex("dbo.Items", "CollectionId");
            CreateIndex("dbo.BoolValues", "ItemId");
            AddForeignKey("dbo.Items", "CollectionId", "dbo.Collections", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tags", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StringValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IntValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.DateValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BoolValues", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
        }
    }
}
