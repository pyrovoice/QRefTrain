namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Exam_Id", c => c.Int());
            AddColumn("dbo.Questions", "Result_Id", c => c.Int());
            AddColumn("dbo.Answers", "Result_Id", c => c.Int());
            CreateIndex("dbo.Questions", "Exam_Id");
            CreateIndex("dbo.Questions", "Result_Id");
            CreateIndex("dbo.Answers", "Result_Id");
            AddForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams", "Id");
            AddForeignKey("dbo.Questions", "Result_Id", "dbo.Results", "Id");
            AddForeignKey("dbo.Answers", "Result_Id", "dbo.Results", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Result_Id", "dbo.Results");
            DropForeignKey("dbo.Questions", "Result_Id", "dbo.Results");
            DropForeignKey("dbo.Questions", "Exam_Id", "dbo.Exams");
            DropIndex("dbo.Answers", new[] { "Result_Id" });
            DropIndex("dbo.Questions", new[] { "Result_Id" });
            DropIndex("dbo.Questions", new[] { "Exam_Id" });
            DropColumn("dbo.Answers", "Result_Id");
            DropColumn("dbo.Questions", "Result_Id");
            DropColumn("dbo.Questions", "Exam_Id");
        }
    }
}
