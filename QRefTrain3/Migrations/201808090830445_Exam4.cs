namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exam4 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Exams", "SecurityHash");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exams", "SecurityHash", c => c.String());
        }
    }
}
