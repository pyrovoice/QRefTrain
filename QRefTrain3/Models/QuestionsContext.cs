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
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasMany(question => question.Answers)
                .WithMany(answer => answer.Questions)
                .Map(relation =>
                {
                    relation.MapLeftKey("Answer_Id");
                    relation.MapRightKey("Question_Id");
                    relation.ToTable("AnswerQuestions");
                });
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Answer> Answers { get; set; }
    }
}