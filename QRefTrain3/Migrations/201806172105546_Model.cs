namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Model : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Field = c.Int(nullable: false),
                        Difficulty = c.Int(nullable: false),
                        IsVideo = c.Boolean(nullable: false),
                        VideoURL = c.String(),
                        QuestionText = c.String(nullable: false),
                        AnswerType = c.Int(nullable: false),
                        AnswerExplanation = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Answertext = c.String(),
                        IsTrue = c.Boolean(nullable: false),
                        IsSelected = c.Boolean(nullable: false),
                        Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ResultType = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 32),
                        Email = c.String(nullable: false),
                        IsEmailConfirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Results", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropIndex("dbo.Results", new[] { "User_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Results");
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
        }
    }
}
