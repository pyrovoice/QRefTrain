namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnswerLogic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Answers", "IsSelected", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Answers", "IsSelected");
        }
    }
}
