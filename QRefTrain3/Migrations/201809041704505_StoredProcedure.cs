namespace QRefTrain3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class StoredProcedure : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("dbo.CountMail", c => new { mail = c.String() }, "SELECT COUNT(*) FROM USERS WHERE users.Email = @mail");
            CreateStoredProcedure("dbo.CreateUser", c => new { name = c.String(), password = c.String(), email = c.String() }, "BEGIN SET NOCOUNT ON INSERT INTO dbo.[Users](name, Password, Email, isEmailConfirmed, RegisterationDate) VALUES(@name, HASHBYTES('MD5', @password), @email, 0, GETDATE()) END");
            CreateStoredProcedure("dbo.IdentifyUser", c => new { name = c.String(), password = c.String() }, "SELECT * from users where users.name = @name AND HASHBYTES('MD5', @password) = users.Password");
            CreateStoredProcedure("dbo.CreateExam", c => new { userId = c.String()}, "BEGIN SET NOCOUNT ON INSERT INTO dbo.[Exams](StartDate, User_Id) VALUES(GETDATE(), @userId) END");
            CreateStoredProcedure("dbo.UpdateUser", c => new { id = c.Int(), name = c.String(), password = c.String(), email = c.String() }, "BEGIN SET NOCOUNT ON UPDATE dbo.[Users] SET Name = @name, Password = HASHBYTES('MD5', @password), Email = @email WHERE Users.Id = @id; END");
            CreateStoredProcedure("dbo.Reset", "BEGIN delete from Answers; delete from QuestionExams; delete from Questions; delete from Exams; delete from Requests; delete from Users; END");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.CountMail");
            DropStoredProcedure("dbo.CreateUser");
            DropStoredProcedure("dbo.IdentifyUser");
            DropStoredProcedure("dbo.CreateExam");
            DropStoredProcedure("dbo.UpdateUser");
            DropStoredProcedure("dbo.Reset");
        }
    }
}
