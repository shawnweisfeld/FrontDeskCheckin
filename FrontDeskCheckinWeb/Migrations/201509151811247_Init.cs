namespace FrontDeskCheckinWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Terminals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(),
                        SiteName = c.String(),
                        Building = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Visitors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Company = c.String(),
                        Sponsor = c.String(),
                        ArrivedAt = c.DateTime(nullable: false),
                        DepartedAt = c.DateTime(nullable: false),
                        Terminal_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Terminals", t => t.Terminal_Id)
                .Index(t => t.Terminal_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Visitors", "Terminal_Id", "dbo.Terminals");
            DropIndex("dbo.Visitors", new[] { "Terminal_Id" });
            DropTable("dbo.Visitors");
            DropTable("dbo.Terminals");
        }
    }
}
