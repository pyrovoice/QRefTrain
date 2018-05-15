namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _14_AddExplanationToQuestions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "AnswerExplanation", c => c.String(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Questions", "AnswerExplanation");
        }
    }
}
