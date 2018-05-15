namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Migration_12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Results",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    User_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Results", "User_Id", "dbo.Users");
            DropIndex("dbo.Results", new[] { "User_Id" });
            DropTable("dbo.Results");
        }
    }
}
