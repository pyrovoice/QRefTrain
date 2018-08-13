namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exam7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Exams", "Result_Id", "dbo.Results");
            DropIndex("dbo.Exams", new[] { "Result_Id" });
            DropColumn("dbo.Exams", "Result_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exams", "Result_Id", c => c.Int());
            CreateIndex("dbo.Exams", "Result_Id");
            AddForeignKey("dbo.Exams", "Result_Id", "dbo.Results", "Id");
        }
    }
}
