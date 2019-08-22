namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VirtualEverywhere : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "User_Id", c => c.Int());
            AlterColumn("dbo.Questions", "AnswerExplanation", c => c.String(nullable: false));
            CreateIndex("dbo.Logs", "User_Id");
            AddForeignKey("dbo.Logs", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Logs", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "UserId", c => c.Int());
            DropForeignKey("dbo.Logs", "User_Id", "dbo.Users");
            DropIndex("dbo.Logs", new[] { "User_Id" });
            AlterColumn("dbo.Questions", "AnswerExplanation", c => c.String());
            DropColumn("dbo.Logs", "User_Id");
        }
    }
}
