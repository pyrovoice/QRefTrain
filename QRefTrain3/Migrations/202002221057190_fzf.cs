namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fzf : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QuizTemplates", "NGB");
            DropColumn("dbo.Users", "Rank");
            DropColumn("dbo.Users", "LastRankPassedDate");
            DropColumn("dbo.Quizs", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Quizs", "Type", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "LastRankPassedDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AddColumn("dbo.Users", "Rank", c => c.Int(nullable: false));
            AddColumn("dbo.QuizTemplates", "NGB", c => c.Int(nullable: false));
        }
    }
}
