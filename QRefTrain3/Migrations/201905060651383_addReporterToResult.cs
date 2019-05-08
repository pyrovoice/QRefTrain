namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReporterToResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "Reporter_Id", c => c.Int());
            CreateIndex("dbo.Results", "Reporter_Id");
            AddForeignKey("dbo.Results", "Reporter_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Results", "Reporter_Id", "dbo.Users");
            DropIndex("dbo.Results", new[] { "Reporter_Id" });
            DropColumn("dbo.Results", "Reporter_Id");
        }
    }
}
