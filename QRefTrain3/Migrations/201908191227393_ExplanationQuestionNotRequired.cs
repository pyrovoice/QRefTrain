namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExplanationQuestionNotRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "AnswerExplanation", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "AnswerExplanation", c => c.String(nullable: false));
        }
    }
}
