namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResultAnswersManyToManyFix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "Result_Id", "dbo.Results");
            DropIndex("dbo.Answers", new[] { "Result_Id" });
            CreateTable(
                "dbo.ResultAnswers",
                c => new
                    {
                        Result_Id = c.Int(nullable: false),
                        Answer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Result_Id, t.Answer_Id })
                .ForeignKey("dbo.Results", t => t.Result_Id, cascadeDelete: true)
                .ForeignKey("dbo.Answers", t => t.Answer_Id, cascadeDelete: true)
                .Index(t => t.Result_Id)
                .Index(t => t.Answer_Id);
            
            DropColumn("dbo.Answers", "Result_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Answers", "Result_Id", c => c.Int());
            DropForeignKey("dbo.ResultAnswers", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.ResultAnswers", "Result_Id", "dbo.Results");
            DropIndex("dbo.ResultAnswers", new[] { "Answer_Id" });
            DropIndex("dbo.ResultAnswers", new[] { "Result_Id" });
            DropTable("dbo.ResultAnswers");
            CreateIndex("dbo.Answers", "Result_Id");
            AddForeignKey("dbo.Answers", "Result_Id", "dbo.Results", "Id");
        }
    }
}
