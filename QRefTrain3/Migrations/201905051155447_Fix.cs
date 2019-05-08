namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionSuites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        code = c.String(),
                        name = c.String(),
                        owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.owner_Id)
                .Index(t => t.owner_Id);
            
            CreateTable(
                "dbo.QuestionSuiteQuestions",
                c => new
                    {
                        QuestionSuite_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.QuestionSuite_Id, t.Question_Id })
                .ForeignKey("dbo.QuestionSuites", t => t.QuestionSuite_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.QuestionSuite_Id)
                .Index(t => t.Question_Id);
            
            AddColumn("dbo.Exams", "SuiteId", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionSuiteQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.QuestionSuiteQuestions", "QuestionSuite_Id", "dbo.QuestionSuites");
            DropForeignKey("dbo.QuestionSuites", "owner_Id", "dbo.Users");
            DropIndex("dbo.QuestionSuiteQuestions", new[] { "Question_Id" });
            DropIndex("dbo.QuestionSuiteQuestions", new[] { "QuestionSuite_Id" });
            DropIndex("dbo.QuestionSuites", new[] { "owner_Id" });
            DropColumn("dbo.Exams", "SuiteId");
            DropTable("dbo.QuestionSuiteQuestions");
            DropTable("dbo.QuestionSuites");
        }
    }
}
