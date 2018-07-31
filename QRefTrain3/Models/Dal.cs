﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace QRefTrain3.Models
{
    public class Dal : IDal
    {
        private static Dal dal;
        private static QuestionsContext Context { get; set; }

        private Dal()
        {
            Context = new QuestionsContext();
        }

        public void CreateUser(User user)
        {
            var affectedRows = Context.Database.ExecuteSqlCommand("CreateUser @name, @password, @email", new SqlParameter("@name", user.Name), new SqlParameter("@password", user.Password), new SqlParameter("@email", user.Email));
            Console.Write("CREATE USER " + user.Name);
        }

        public List<Question> GetQuestionByIds(List<int> questionsAskedIds)
        {
            List<Question> returnList = new List<Question>();
            foreach (Question q in Context.Questions)
            {
                if (questionsAskedIds.Contains(q.Id))
                {
                    returnList.Add(q);
                }
            }
            return returnList;
        }

        public List<Result> GetNLastResultByUser(User user, int number)
        {
            List<Result> results = GetResultByUser(user);
            return results.OrderByDescending(o => o.DateTime).ToList().Take(number).ToList();

        }

        public List<Question> GetQuestionsByParameter(List<string> fields, List<string> difficulties, NationalGoverningBody body)
        {
            return Context.Questions.Where<Question>(q => fields.Contains(q.Field.ToString()) && (fields.Contains("Any") || fields.Contains(q.Field.ToString())) && q.NationalGoverningBodies.Contains(body.ToString())).ToList<Question>();
        }

        public bool UsernameAlreadyInDB(string name)
        {
            var a = Context.Users.Where(stringToCheck => stringToCheck.Name.Equals(name));
            return a.Count() > 0;
        }

        public bool MailAlreadyInDB(string email)
        {
            var result = Context.Database.SqlQuery<int>("CountMail @mail", new SqlParameter("mail", email));
            int count = result.First();
            return count != 0;
        }

        public bool UserExistsInDB(User user)
        {
            User foundUser = Context.Users.First<User>(u => u.Name == user.Name || u.Email == user.Email);
            return user != null ? true : false;
        }

        public Result GetResultById(int resultId)
        {
            return Context.Results.FirstOrDefault(r => r.Id == resultId);
        }

        public List<Result> GetResultByUser(User user)
        {
            List<Result> results = new List<Result>();
            foreach (Result result in Context.Results)
            {
                if (result.User.Id == user.Id)
                {
                    results.Add(result);
                }
            }
            return results;
        }

        public User GetUserByName(string name)
        {
            return Context.Users.FirstOrDefault(u => u.Name.Equals(name));
        }

        public Question CreateQuestion(Question question)
        {
            Context.Questions.Add(question);
            Context.SaveChanges();
            return question;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public List<Question> getAllQuestions()
        {
            return Context.Questions.ToList();
        }

        public List<User> getAllUsers()
        {
            return Context.Users.ToList();
        }

        public void DeleteUser(User user)
        {
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        public void DeleteQuestion(Question question)
        {
            Context.Questions.Remove(question);
            Context.SaveChanges();
        }

        public void reset()
        {
            Context.Database.ExecuteSqlCommand("delete from Answers");
            Context.Database.ExecuteSqlCommand("delete from Results");
            Context.Database.ExecuteSqlCommand("delete from Questions");
            Context.Database.ExecuteSqlCommand("delete from Users");

        }

        public void CreateResult(Result result)
        {
            Context.Results.Add(result);
            Context.SaveChanges();
        }

        public Question GetQuestionById(int id)
        {
            return Context.Questions.Single(q => q.Id == id);
        }

        public static Dal Instance
        {
            get
            {
                if (dal == null)
                {
                    dal = new Dal();
                }
                return dal;
            }
        }

        public void AlterQuestion(Question question)
        {
            Question questionToReplace = Context.Questions.Find(question.Id);
            questionToReplace.Name = question.Name;
            questionToReplace.Field = question.Field;
            Context.SaveChanges();
        }
        public User Authenticate(string userName, string userPassword)
        {
            if (userName == null || userName.Equals("") || userPassword == null || userPassword.Equals(""))
            {
                return null;
            }
            DbRawSqlQuery<User> affectedRows = Context.Database.SqlQuery<User>("IdentifyUser @name, @password", new SqlParameter("@name", userName), new SqlParameter("@password", userPassword));
            return affectedRows.FirstOrDefault();
        }

    }
}