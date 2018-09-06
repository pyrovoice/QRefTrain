namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionAnswersManyToManyFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            CreateTable(
                "dbo.AnswerQuestions",
                c => new
                    {
                        Answer_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Answer_Id, t.Question_Id })
                .ForeignKey("dbo.Answers", t => t.Answer_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Answer_Id)
                .Index(t => t.Question_Id);
            
            DropColumn("dbo.Answers", "Question_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "Question_Id", c => c.Int());
            DropForeignKey("dbo.AnswerQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.AnswerQuestions", "Answer_Id", "dbo.Answers");
            DropIndex("dbo.AnswerQuestions", new[] { "Question_Id" });
            DropIndex("dbo.AnswerQuestions", new[] { "Answer_Id" });
            DropTable("dbo.AnswerQuestions");
            CreateIndex("dbo.Answers", "Question_Id");
            AddForeignKey("dbo.Answers", "Question_Id", "dbo.Questions", "Id");
        }
    }
}
