namespace CourseProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CollectionId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "Collection_Id", "dbo.Collections");
            DropIndex("dbo.Items", new[] { "Collection_Id" });
            RenameColumn(table: "dbo.Items", name: "Collection_Id", newName: "CollectionId");
            AlterColumn("dbo.Items", "CollectionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", "CollectionId");
            AddForeignKey("dbo.Items", "CollectionId", "dbo.Collections", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "CollectionId", "dbo.Collections");
            DropIndex("dbo.Items", new[] { "CollectionId" });
            AlterColumn("dbo.Items", "CollectionId", c => c.Int());
            RenameColumn(table: "dbo.Items", name: "CollectionId", newName: "Collection_Id");
            CreateIndex("dbo.Items", "Collection_Id");
            AddForeignKey("dbo.Items", "Collection_Id", "dbo.Collections", "Id");
        }
    }
}
