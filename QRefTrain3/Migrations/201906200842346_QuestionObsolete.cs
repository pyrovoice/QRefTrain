namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionObsolete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "IsRetired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "IsRetired");
        }
    }
}
