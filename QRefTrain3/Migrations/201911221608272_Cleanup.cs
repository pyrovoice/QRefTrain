namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cleanup : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.QuestionSuites", newName: "QuizTemplates");
            RenameTable(name: "dbo.Exams", newName: "Quizs");
            RenameTable(name: "dbo.QuestionSuiteQuestions", newName: "QuizTemplateQuestions");
            RenameColumn(table: "dbo.QuizTemplateQuestions", name: "QuestionSuite_Id", newName: "QuizTemplate_Id");
            RenameIndex(table: "dbo.QuizTemplateQuestions", name: "IX_QuestionSuite_Id", newName: "IX_QuizTemplate_Id");
            AddColumn("dbo.Results", "QuizType", c => c.Int(nullable: false));
            DropColumn("dbo.Results", "ResultType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Results", "ResultType", c => c.Int(nullable: false));
            DropColumn("dbo.Results", "QuizType");
            RenameIndex(table: "dbo.QuizTemplateQuestions", name: "IX_QuizTemplate_Id", newName: "IX_QuestionSuite_Id");
            RenameColumn(table: "dbo.QuizTemplateQuestions", name: "QuizTemplate_Id", newName: "QuestionSuite_Id");
            RenameTable(name: "dbo.QuizTemplateQuestions", newName: "QuestionSuiteQuestions");
            RenameTable(name: "dbo.Quizs", newName: "Exams");
            RenameTable(name: "dbo.QuizTemplates", newName: "QuestionSuites");
        }
    }
}
