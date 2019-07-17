namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timeLimit : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.QuestionSuites", new[] { "owner_Id" });
            AddColumn("dbo.Exams", "TimeLimit", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionSuites", "TimeLimit", c => c.Int(nullable: false));
            CreateIndex("dbo.QuestionSuites", "Owner_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.QuestionSuites", new[] { "Owner_Id" });
            DropColumn("dbo.QuestionSuites", "TimeLimit");
            DropColumn("dbo.Exams", "TimeLimit");
            CreateIndex("dbo.QuestionSuites", "owner_Id");
        }
    }
}
