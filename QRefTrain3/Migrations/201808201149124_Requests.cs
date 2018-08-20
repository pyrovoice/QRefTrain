namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Requests : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestType = c.Int(nullable: false),
                        SecretCode = c.String(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Users", "RegisterationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "User_Id", "dbo.Users");
            DropIndex("dbo.Requests", new[] { "User_Id" });
            DropColumn("dbo.Users", "RegisterationDate");
            DropTable("dbo.Requests");
        }
    }
}
