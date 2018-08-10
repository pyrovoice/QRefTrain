namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exam3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exams", "SecurityHash", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exams", "SecurityHash");
        }
    }
}
