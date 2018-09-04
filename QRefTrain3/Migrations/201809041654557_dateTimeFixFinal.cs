namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTimeFixFinal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logs", "LogTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Logs", "LogTime", c => c.DateTime(nullable: false));
        }
    }
}
