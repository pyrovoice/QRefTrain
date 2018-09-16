namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Answertext = c.String(),
                        IsTrue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Subject = c.Int(nullable: false),
                        IsVideo = c.Boolean(nullable: false),
                        VideoURL = c.String(),
                        QuestionText = c.String(nullable: false),
                        AnswerExplanation = c.String(nullable: false),
                        NationalGoverningBodies = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                        Password = c.String(nullable: false, maxLength: 32),
                        Email = c.String(nullable: false),
                        IsEmailConfirmed = c.Boolean(nullable: false),
                        RegisterationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LogTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.Int(),
                        LogText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RequestType = c.Int(nullable: false),
                        SecretCode = c.String(nullable: false, maxLength: 32),
                        CreationDate = c.DateTime(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        Question_Id = c.Int(nullable: false),
                        Answer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Question_Id, t.Answer_Id })
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .ForeignKey("dbo.Answers", t => t.Answer_Id, cascadeDelete: true)
                .Index(t => t.Question_Id)
                .Index(t => t.Answer_Id);
            
            CreateTable(
                "dbo.ExamQuestions",
                c => new
                    {
                        Exam_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Exam_Id, t.Question_Id })
                .ForeignKey("dbo.Exams", t => t.Exam_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Exam_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.ResultQuestions",
                c => new
                    {
                        Result_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Result_Id, t.Question_Id })
                .ForeignKey("dbo.Results", t => t.Result_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Result_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.ResultAnswers",
                c => new
                    {
                        Result_Id = c.Int(nullable: false),
                        Answer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Result_Id, t.Answer_Id })
                .ForeignKey("dbo.Results", t => t.Result_Id, cascadeDelete: true)
                .ForeignKey("dbo.Answers", t => t.Answer_Id, cascadeDelete: true)
                .Index(t => t.Result_Id)
                .Index(t => t.Answer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Results", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ResultAnswers", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.ResultAnswers", "Result_Id", "dbo.Results");
            DropForeignKey("dbo.ResultQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.ResultQuestions", "Result_Id", "dbo.Results");
            DropForeignKey("dbo.Exams", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ExamQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.ExamQuestions", "Exam_Id", "dbo.Exams");
            DropForeignKey("dbo.QuestionAnswers", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.QuestionAnswers", "Question_Id", "dbo.Questions");
            DropIndex("dbo.ResultAnswers", new[] { "Answer_Id" });
            DropIndex("dbo.ResultAnswers", new[] { "Result_Id" });
            DropIndex("dbo.ResultQuestions", new[] { "Question_Id" });
            DropIndex("dbo.ResultQuestions", new[] { "Result_Id" });
            DropIndex("dbo.ExamQuestions", new[] { "Question_Id" });
            DropIndex("dbo.ExamQuestions", new[] { "Exam_Id" });
            DropIndex("dbo.QuestionAnswers", new[] { "Answer_Id" });
            DropIndex("dbo.QuestionAnswers", new[] { "Question_Id" });
            DropIndex("dbo.Requests", new[] { "User_Id" });
            DropIndex("dbo.Results", new[] { "User_Id" });
            DropIndex("dbo.Exams", new[] { "User_Id" });
            DropTable("dbo.ResultAnswers");
            DropTable("dbo.ResultQuestions");
            DropTable("dbo.ExamQuestions");
            DropTable("dbo.QuestionAnswers");
            DropTable("dbo.Requests");
            DropTable("dbo.Logs");
            DropTable("dbo.Results");
            DropTable("dbo.Users");
            DropTable("dbo.Exams");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
