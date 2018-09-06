namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionResultManyToManyFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "Result_Id", "dbo.Results");
            DropIndex("dbo.Questions", new[] { "Result_Id" });
            CreateTable(
                "dbo.ResultQuestions",
                c => new
                    {
                        Result_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Result_Id, t.Question_Id })
                .ForeignKey("dbo.Results", t => t.Result_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Result_Id)
                .Index(t => t.Question_Id);
            
            DropColumn("dbo.Questions", "Result_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Result_Id", c => c.Int());
            DropForeignKey("dbo.ResultQuestions", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.ResultQuestions", "Result_Id", "dbo.Results");
            DropIndex("dbo.ResultQuestions", new[] { "Question_Id" });
            DropIndex("dbo.ResultQuestions", new[] { "Result_Id" });
            DropTable("dbo.ResultQuestions");
            CreateIndex("dbo.Questions", "Result_Id");
            AddForeignKey("dbo.Questions", "Result_Id", "dbo.Results", "Id");
        }
    }
}
