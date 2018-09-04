namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateTimeFix3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "RegisterationDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Results", "DateTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Results", "DateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "RegisterationDate", c => c.DateTime(nullable: false));
        }
    }
}
