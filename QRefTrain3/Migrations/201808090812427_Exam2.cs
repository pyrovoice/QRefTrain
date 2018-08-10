namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exam2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exams", "User_Id", "dbo.Users");
            DropIndex("dbo.Exams", new[] { "User_Id" });
            DropTable("dbo.Exams");
        }
    }
}
