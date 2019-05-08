using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class QuestionsContext : DbContext
    {
        public QuestionsContext()
        {
            Database.SetInitializer<QuestionsContext>(null);
            //Database.SetInitializer<QuestionsContext>(new DropCreateDatabaseIfModelChanges<QuestionsContext>());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<QuestionSuite> QuestionSuites { get; set; }
    }
}