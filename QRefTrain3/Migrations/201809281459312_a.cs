namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ResultQuestions", newName: "QuestionResults");
            DropForeignKey("dbo.AnswerQuestions", "Answer_Id", "dbo.Questions");
            DropForeignKey("dbo.AnswerQuestions", "Question_Id", "dbo.Answers");
            DropIndex("dbo.AnswerQuestions", new[] { "Answer_Id" });
            DropIndex("dbo.AnswerQuestions", new[] { "Question_Id" });
            DropPrimaryKey("dbo.QuestionResults");
            AddColumn("dbo.Answers", "Question_Id", c => c.Int());
            AddPrimaryKey("dbo.QuestionResults", new[] { "Question_Id", "Result_Id" });
            CreateIndex("dbo.Answers", "Question_Id");
            AddForeignKey("dbo.Answers", "Question_Id", "dbo.Questions", "Id");
            DropTable("dbo.AnswerQuestions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AnswerQuestions",
                c => new
                    {
                        Answer_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Answer_Id, t.Question_Id });
            
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropPrimaryKey("dbo.QuestionResults");
            DropColumn("dbo.Answers", "Question_Id");
            AddPrimaryKey("dbo.QuestionResults", new[] { "Result_Id", "Question_Id" });
            CreateIndex("dbo.AnswerQuestions", "Question_Id");
            CreateIndex("dbo.AnswerQuestions", "Answer_Id");
            AddForeignKey("dbo.AnswerQuestions", "Question_Id", "dbo.Answers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AnswerQuestions", "Answer_Id", "dbo.Questions", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.QuestionResults", newName: "ResultQuestions");
        }
    }
}
