namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Migration11 : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.Questions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Field = c.Int(nullable: false),
                    Difficulty = c.Int(nullable: false),
                    IsVideo = c.Boolean(nullable: false),
                    VideoURL = c.String(),
                    QuestionText = c.String(nullable: false),
                    AnswerType = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Answers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Answertext = c.String(),
                    IsTrue = c.Boolean(nullable: false),
                    IsSelected = c.Boolean(nullable: false),
                    Question_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.Question_Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Password = c.String(nullable: false),
                    Email = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropTable("dbo.Users");
            DropTable("dbo.Answers");
            DropTable("dbo.Questions");
        }
    }
}
