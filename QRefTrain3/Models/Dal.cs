﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Validation;
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
            Context.Database.ExecuteSqlCommand("CreateUser @name, @password, @email", new SqlParameter("@name", user.Name), new SqlParameter("@password", user.Password), new SqlParameter("@email", user.Email));
        }

        public List<QuizTemplate> GetQuestionSuiteByUser(User currentUser)
        {
            return Context.QuestionSuites.Where(q => q.Owner.Id == currentUser.Id).ToList();
        }

        public List<Result> GetNLastResultByUser(User user, int number)
        {
            List<Result> results = GetResultByUser(user);
            return results.OrderByDescending(o => o.DateTime).ToList().Take(number).ToList();
        }

        public User GetUserByMail(string userMail)
        {
            return Context.Users.FirstOrDefault(u => u.Email.Equals(userMail));
        }

        internal object GetQuestionSuiteByCode(string code)
        {
            return Context.QuestionSuites.FirstOrDefault(u => u.Code.Equals(code));
        }

        internal List<Question> GetQuestionsById(List<string> questionIds)
        {
            return Context.Questions.Where<Question>(q => questionIds.Contains(q.Id.ToString())).ToList<Question>();
        }

        internal List<Log> getAllLogs()
        {
            return Context.Logs.ToList();
        }

        public QuizTemplate CreateTemporaryQuizTemplate(QuizTemplate newQuestionSuite)
        {
            Context.QuestionSuites.Add(newQuestionSuite);
            Context.SaveChanges();
            return newQuestionSuite;
        }



        public List<Answer> getAllAnswers()
        {
            return Context.Answers.ToList();
        }

        public QuizTemplate GetQuestionSuiteByString(string questionSuiteText)
        {
            var qs = Context.QuestionSuites.FirstOrDefault(s => s.Code.Equals(questionSuiteText));
            return qs;
        }

        internal void DeleteQuestionSuiteById(int suiteId)
        {
            QuizTemplate qs = Context.QuestionSuites.FirstOrDefault(q => q.Id == suiteId);
            if (qs != null)
            {
                Context.QuestionSuites.Remove(qs);
                Context.SaveChanges();
            }
        }



        public Answer GetAnswer(string answerTitle, bool isAnswerTrue)
        {
            return Context.Answers.FirstOrDefault(a => a.Answertext.Equals(answerTitle) && a.IsTrue == isAnswerTrue);
        }

        public Request GetRequestByCode(string code)
        {
            return Context.Requests.FirstOrDefault(q => q.SecretCode.Equals(code));
        }

        public Quiz GetOngoingExamByUsername(string userName)
        {
            User user = GetUserByName(userName);
            if (user == null)
            {
                return null;
            }
            Quiz exam = Context.Exams.FirstOrDefault(e => e.User.Id == user.Id && e.IsClosed == false);
            return exam;
        }

        internal void UpdateUserChangePassword(User user, string newPassword)
        {
            Context.Database.ExecuteSqlCommand("UpdateUser @id, @name, @password, @email", new SqlParameter("@id", user.Id), new SqlParameter("@name", user.Name), new SqlParameter("@password", newPassword), new SqlParameter("@email", user.Email));
        }

        public Request CreateRequest(Request request)
        {
            Context.Requests.Add(request);
            Context.SaveChanges();
            return request;
        }

        /// <summary>
        /// Creates a result object for the user for each of his unfinished exam, with a result of 0.
        /// </summary>
        /// <param name="userName"></param>
        public void CloseExamByUsername(string userName)
        {
            User user = GetUserByName(userName);
            foreach (Quiz ongoingExam in Context.Exams.Where(q => q.User.Name.Equals(userName)).ToList())
            {
                if (ongoingExam.Suite != null)
                {
                    Result result = new Result(user, ongoingExam.Suite.Questions, new List<Answer>(), QuizType.Exam, GetDBTime(), ongoingExam.Suite);
                }
                ongoingExam.IsClosed = true;
            }
            Context.SaveChanges();
        }

        public List<Question> GetQuestionsByParameter(string ngb)
        {
            return Context.Questions.Where<Question>(q =>
                (q.NationalGoverningBodies.Contains(ngb) || q.NationalGoverningBodies == NationalGoverningBody.All.ToString()) && q.IsRetired == false)
                .ToList<Question>();
        }

        public List<Question> GetQuestionsByParameter(string NGB, List<string> subjects)
        {
            if (subjects == null)
            {
                return GetQuestionsByParameter(NGB);
            }
            return Context.Questions.Where<Question>(q =>
            (subjects.Contains(q.Subject.ToString()))
            && (q.NationalGoverningBodies.Contains(NGB) || q.NationalGoverningBodies == NationalGoverningBody.All.ToString()) && q.IsRetired == false)
            .ToList<Question>();
        }

        public List<Question> GetQuestionsByNGB(string NGB)
        {
            return Context.Questions.Where<Question>(q => q.NationalGoverningBodies.Equals("All") || q.NationalGoverningBodies.Contains(NGB)).ToList<Question>();
        }

        public bool IsUsernameAlreadyInDB(string name)
        {
            var a = Context.Users.Where(stringToCheck => stringToCheck.Name.Equals(name));
            return a.Count() > 0;
        }

        public bool IsMailAlreadyInDB(string email)
        {
            var result = Context.Database.SqlQuery<int>("CountMail @mail", new SqlParameter("mail", email));
            int count = result.First();
            return count != 0;
        }

        public Request GetRequestByCodeAndId(string code, int id)
        {
            return Context.Requests.FirstOrDefault(r => r.Id == id && r.SecretCode.Equals(code));
        }

        public void UpdateUserConfirmMail(User user)
        {
            User userToUpdate = Context.Users.FirstOrDefault(u => u.Id == user.Id);
            userToUpdate.IsEmailConfirmed = true;
            Context.SaveChanges();
        }

        public void DeleteRequest(Request request)
        {
            Context.Requests.Remove(request);
            Context.SaveChanges();
        }

        public Answer GetAnswerById(int id)
        {
            return Context.Answers.FirstOrDefault(q => q.Id == id);
        }

        /// <summary>
        /// Creates a temporary question suite then create an exam using this suite.
        /// </summary>
        /// <param name="name">User's name that play the exam</param>
        /// <param name="timeNow">Time at the creation of the exam</param>
        /// <param name="timeLimit">How long after the start does the exam closes</param>
        /// <param name="questions">A list of question used to created and link to a suite</param>
        /// <returns>The newly created exam</returns>
        public Quiz CreateQuiz(string name, int timeLimit, List<Question> questions)
        {
            var qs = CreateTemporaryQuizTemplate(new QuizTemplate(questions, null, "Temporary", timeLimit));
            return CreateQuiz(name, qs);
        }

        public Quiz CreateQuiz(string name, QuizTemplate quizTemplate)
        {
            User user = GetUserByName(name);
            Quiz exam = new Quiz(GetDBTime(), user, quizTemplate);
            Context.Exams.Add(exam);
            Context.SaveChanges();
            return exam;
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
                if (result.User != null && result.User.Id == user.Id)
                {
                    results.Add(result);
                }
            }
            return results;
        }

        public Quiz GetExamById(int examId)
        {
            return Context.Exams.FirstOrDefault(exam => exam.Id == examId);
        }

        public User GetUserByName(string name)
        {
            //return Context.Database.SqlQuery<User>("Select * from Users where name = @name", new SqlParameter("name", name)).FirstOrDefault();
            return Context.Users.FirstOrDefault(u => u.Name.Equals(name));
        }

        public Question CreateQuestion(Question question)
        {
            Context.Questions.Add(question);
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
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
            Context.Database.ExecuteSqlCommand("delete from Exams");
            Context.Database.ExecuteSqlCommand("delete from Answers");
            Context.Database.ExecuteSqlCommand("delete from Results");
            Context.Database.ExecuteSqlCommand("delete from Questions");
            Context.Database.ExecuteSqlCommand("delete from Users");

        }

        public QuizTemplate GetQuestionSuiteById(int value)
        {
            return Context.QuestionSuites.FirstOrDefault(s => s.Id == value);
        }

        /// <summary>
        /// Return the datetime by using datatable's time and applying user's timezone. This is used to prevent cheats where the user changes his computer's time to alter a result
        /// </summary>
        /// <returns></returns>
        public DateTime GetDBTime()
        {
            return new DateTime(Context.Database.SqlQuery<DateTime>("select GETDATE()").First().Ticks).ToLocalTime();
        }

        public void CreateResult(Result result)
        {
            result.DateTime = GetDBTime();
            Context.Results.Add(result);
            Context.SaveChanges();
        }

        public void CreateLog(Log log)
        {
            Context.Logs.Add(log);
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

        public void DeleteExamByUserId(int userId)
        {
            foreach (Quiz e in Context.Exams)
            {
                if (e.User == null || e.User.Id == userId)
                {
                    Context.Exams.Remove(e);
                    if (e.Suite != null && e.Suite.Owner == null)
                    {
                        DeleteQuestionSuiteById(e.Suite.Id);
                    }
                }
            }
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

        public List<Question> getAllQuestionsExceptRetired()
        {
            return Context.Questions.Where(q => q.IsRetired == false).ToList();
        }

        public void RetireQuestion(Question question)
        {
            Question q2 = Context.Questions.Single(q => q.Id == question.Id);
            q2.IsRetired = true;
            Context.SaveChanges();
        }
    }
}