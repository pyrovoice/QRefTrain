namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fff : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "Suite_Id", c => c.Int());
            CreateIndex("dbo.Exams", "Suite_Id");
            AddForeignKey("dbo.Exams", "Suite_Id", "dbo.QuestionSuites", "Id");
            DropColumn("dbo.Exams", "SuiteId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exams", "SuiteId", c => c.Int());
            DropForeignKey("dbo.Exams", "Suite_Id", "dbo.QuestionSuites");
            DropIndex("dbo.Exams", new[] { "Suite_Id" });
            DropColumn("dbo.Exams", "Suite_Id");
        }
    }
}
