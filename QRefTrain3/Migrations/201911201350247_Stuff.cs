namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Stuff : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Results", "Reporter_Id", "dbo.Users");
            DropIndex("dbo.Results", new[] { "Reporter_Id" });
            AddColumn("dbo.Results", "QuestionSuite_Id", c => c.Int());
            AddColumn("dbo.Exams", "IsClosed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Logs", "Level", c => c.Int(nullable: false));
            CreateIndex("dbo.Results", "QuestionSuite_Id");
            AddForeignKey("dbo.Results", "QuestionSuite_Id", "dbo.QuestionSuites", "Id");
            DropColumn("dbo.Results", "Reporter_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Results", "Reporter_Id", c => c.Int());
            DropForeignKey("dbo.Results", "QuestionSuite_Id", "dbo.QuestionSuites");
            DropIndex("dbo.Results", new[] { "QuestionSuite_Id" });
            DropColumn("dbo.Logs", "Level");
            DropColumn("dbo.Exams", "IsClosed");
            DropColumn("dbo.Results", "QuestionSuite_Id");
            CreateIndex("dbo.Results", "Reporter_Id");
            AddForeignKey("dbo.Results", "Reporter_Id", "dbo.Users", "Id");
        }
    }
}
