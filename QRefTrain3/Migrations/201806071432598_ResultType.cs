namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResultType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "ResultType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Results", "ResultType");
        }
    }
}
