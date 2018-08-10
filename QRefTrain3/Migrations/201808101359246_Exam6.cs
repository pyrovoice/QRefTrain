namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exam6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogTime = c.DateTime(nullable: false),
                        UserId = c.Int(),
                        LogText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Exams", "Result_Id", c => c.Int());
            CreateIndex("dbo.Exams", "Result_Id");
            AddForeignKey("dbo.Exams", "Result_Id", "dbo.Results", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exams", "Result_Id", "dbo.Results");
            DropIndex("dbo.Exams", new[] { "Result_Id" });
            DropColumn("dbo.Exams", "Result_Id");
            DropTable("dbo.Logs");
        }
    }
}
