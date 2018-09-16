namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wut : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.QuestionAnswers", newName: "AnswerQuestions");
            RenameColumn(table: "dbo.AnswerQuestions", name: "Question_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.AnswerQuestions", name: "Answer_Id", newName: "Question_Id");
            RenameColumn(table: "dbo.AnswerQuestions", name: "__mig_tmp__0", newName: "Answer_Id");
            RenameIndex(table: "dbo.AnswerQuestions", name: "IX_Question_Id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.AnswerQuestions", name: "IX_Answer_Id", newName: "IX_Question_Id");
            RenameIndex(table: "dbo.AnswerQuestions", name: "__mig_tmp__0", newName: "IX_Answer_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AnswerQuestions", name: "IX_Answer_Id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.AnswerQuestions", name: "IX_Question_Id", newName: "IX_Answer_Id");
            RenameIndex(table: "dbo.AnswerQuestions", name: "__mig_tmp__0", newName: "IX_Question_Id");
            RenameColumn(table: "dbo.AnswerQuestions", name: "Answer_Id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.AnswerQuestions", name: "Question_Id", newName: "Answer_Id");
            RenameColumn(table: "dbo.AnswerQuestions", name: "__mig_tmp__0", newName: "Question_Id");
            RenameTable(name: "dbo.AnswerQuestions", newName: "QuestionAnswers");
        }
    }
}
