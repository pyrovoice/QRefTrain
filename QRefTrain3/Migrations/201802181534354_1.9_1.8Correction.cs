namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _19_18Correction : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id1" });
            DropColumn("dbo.Answers", "Question_Id1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "Question_Id1", c => c.Int());
            CreateIndex("dbo.Answers", "Question_Id1");
            AddForeignKey("dbo.Answers", "Question_Id1", "dbo.Questions", "Id");
        }
    }
}
