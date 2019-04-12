namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGif : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "GifName", c => c.String());
            DropColumn("dbo.Questions", "Name");
            DropColumn("dbo.Questions", "IsVideo");
            DropColumn("dbo.Questions", "VideoURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "VideoURL", c => c.String());
            AddColumn("dbo.Questions", "IsVideo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Questions", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Questions", "GifName");
        }
    }
}
