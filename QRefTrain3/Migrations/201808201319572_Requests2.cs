namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Requests2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requests", "SecretCode", c => c.String(nullable: false, maxLength: 32));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "SecretCode", c => c.String(nullable: false));
        }
    }
}
