namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class publicId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "PublicId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "PublicId");
        }
    }
}
