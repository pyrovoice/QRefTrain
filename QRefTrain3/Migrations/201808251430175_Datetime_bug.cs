namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datetime_bug : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "RegisterationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "RegisterationDate", c => c.DateTime(nullable: false));
        }
    }
}
