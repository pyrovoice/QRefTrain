namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _18_ChangeQuestionAnswers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "Question_Id1", c => c.Int());
            CreateIndex("dbo.Answers", "Question_Id1");
            AddForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions", "Id");
            DropColumn("dbo.Answers", "IsSelected");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "IsSelected", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id1" });
            DropColumn("dbo.Answers", "Question_Id1");
        }
    }
}
