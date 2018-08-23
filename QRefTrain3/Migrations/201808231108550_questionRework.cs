namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class questionRework : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Subject", c => c.Int(nullable: false));
            DropColumn("dbo.Questions", "Field");
            DropColumn("dbo.Questions", "Difficulty");
            DropColumn("dbo.Questions", "AnswerType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "AnswerType", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "Difficulty", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "Field", c => c.Int(nullable: false));
            DropColumn("dbo.Questions", "Subject");
        }
    }
}
