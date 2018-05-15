namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _17_AddConfirmationMail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsEmailConfirmed", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Users", "IsEmailConfirmed");
        }
    }
}
