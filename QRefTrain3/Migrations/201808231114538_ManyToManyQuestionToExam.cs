namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyQuestionToExam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams");
            DropIndex("dbo.Questions", new[] { "Exam_Id" });
            CreateTable(
                "dbo.QuestionExams",
                c => new
                    {
                        Question_Id = c.Int(nullable: false),
                        Exam_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Question_Id, t.Exam_Id })
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .ForeignKey("dbo.Exams", t => t.Exam_Id, cascadeDelete: true)
                .Index(t => t.Question_Id)
                .Index(t => t.Exam_Id);
            
            DropColumn("dbo.Questions", "Exam_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Exam_Id", c => c.Int());
            DropForeignKey("dbo.QuestionExams", "Exam_Id", "dbo.Exams");
            DropForeignKey("dbo.QuestionExams", "Question_Id", "dbo.Questions");
            DropIndex("dbo.QuestionExams", new[] { "Exam_Id" });
            DropIndex("dbo.QuestionExams", new[] { "Question_Id" });
            DropTable("dbo.QuestionExams");
            CreateIndex("dbo.Questions", "Exam_Id");
            AddForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams", "Id");
        }
    }
}
