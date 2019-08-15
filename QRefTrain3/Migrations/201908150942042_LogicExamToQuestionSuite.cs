namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogicExamToQuestionSuite : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExamQuestions", "Exam_Id", "dbo.Exams");
            DropForeignKey("dbo.ExamQuestions", "Question_Id", "dbo.Questions");
            DropIndex("dbo.ExamQuestions", new[] { "Exam_Id" });
            DropIndex("dbo.ExamQuestions", new[] { "Question_Id" });
            DropColumn("dbo.Exams", "TimeLimit");
            DropTable("dbo.ExamQuestions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ExamQuestions",
                c => new
                    {
                        Exam_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Exam_Id, t.Question_Id });
            
            AddColumn("dbo.Exams", "TimeLimit", c => c.Int(nullable: false));
            CreateIndex("dbo.ExamQuestions", "Question_Id");
            CreateIndex("dbo.ExamQuestions", "Exam_Id");
            AddForeignKey("dbo.ExamQuestions", "Question_Id", "dbo.Questions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ExamQuestions", "Exam_Id", "dbo.Exams", "Id", cascadeDelete: true);
        }
    }
}
