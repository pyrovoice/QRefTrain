namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _15_CorrectDateTimeInResult : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Results", "DateTime");
            AddColumn("dbo.Results", "DateTime", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }

        public override void Down()
        {
            AlterColumn("dbo.Results", "DateTime", c => c.DateTime(nullable: false));
        }
    }
}
