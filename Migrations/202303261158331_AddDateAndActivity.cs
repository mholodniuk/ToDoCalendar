namespace ToDoCalendar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateAndActivity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartTime = c.String(),
                        Done = c.Boolean(nullable: false),
                        DateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Dates", t => t.DateID, cascadeDelete: true)
                .Index(t => t.DateID);
            
            CreateTable(
                "dbo.Dates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Day = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.Students");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Activities", "DateID", "dbo.Dates");
            DropIndex("dbo.Activities", new[] { "DateID" });
            DropTable("dbo.Dates");
            DropTable("dbo.Activities");
        }
    }
}
