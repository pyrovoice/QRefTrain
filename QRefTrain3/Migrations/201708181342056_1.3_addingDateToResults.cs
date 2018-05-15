namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _13_addingDateToResults : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Results", "DateTime", c => c.DateTime(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Results", "DateTime");
        }
    }
}
