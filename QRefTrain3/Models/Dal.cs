using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
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

        /// <summary>
        /// Returns a list of all questions responding to the given parameters (AND tests)
        /// </summary>
        /// <param name="fields">Fields of the selected questions. Any number.</param>
        /// <param name="difficulties">Difficulties of the selected questions. Any number.</param>
        /// <param name="NGB">Chosen NGB. Alway one.</param>
        /// <param name="NGB_Only">Whether we retrieve questions that apply to all NGB or to this particular NGB (some questions might be specific to multiple NGBs, and will be returned)</param>
        /// <returns></returns>
        public List<Question> GetQuestionsByParameter(List<string> fields, List<string> difficulties, string NGB, bool NGB_Only)
        {
            return Context.Questions.Where<Question>(q =>
            fields.Contains(q.Field.ToString())
            && difficulties.Contains(q.Difficulty.ToString())
            && (q.NationalGoverningBodies.Contains(NGB) || (NGB_Only == false && q.NationalGoverningBodies == NationalGoverningBody.All.ToString())))
            .ToList<Question>();
        }

        public Exam GetOngoingExamByUsername(string userName)
        {
            DateTime dbTime = GetDBTime();
            return Context.Exams.FirstOrDefault<Exam>(exam => exam.User.Name.Equals(userName) && SqlFunctions.DateDiff("minute", dbTime, exam.StartDate) <= 10);
        }

        /// <summary>
        /// Creates a result object for the user for each of his unfinished exam, with a result of 0.
        /// </summary>
        /// <param name="userName"></param>
        public void CloseExamByUsername(string userName)
        {
            User  user= GetUserByName(userName);
            foreach (Exam ongoingExam in Context.Exams.Where(q => q.User.Name.Equals(userName)).ToList())
            {
                Result result = new Result()
                {
                    DateTime = GetDBTime(),
                    QuestionsAskedIds = ongoingExam.QuestionsIds,
                    ResultType = ResultType.Exam,
                    SelectedAnswers = new List<int>(),
                    User = user
                };
            }
        }

        public List<Question> GetQuestionsByNGB(string NGB)
        {
            return Context.Questions.Where<Question>(q => q.NationalGoverningBodies.Equals("All") || q.NationalGoverningBodies.Contains(NGB)).ToList<Question>();
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

        public Exam CreateExam(string name, List<Question> questions, DateTime timeNow)
        {
            User user = GetUserByName(name);
            List<int> questionIds = questions.Select(q => q.Id).ToList();

            Exam exam = new Exam()
            {
                QuestionsIds = questionIds,
                StartDate = timeNow,
                User = user
            };
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
                if (result.User.Id == user.Id)
                {
                    results.Add(result);
                }
            }
            return results;
        }

        public Exam GetOngoingExamById(int examId)
        {
            return Context.Exams.FirstOrDefault(exam => exam.Id == examId);
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
            Context.Database.ExecuteSqlCommand("delete from Exams");
            Context.Database.ExecuteSqlCommand("delete from Answers");
            Context.Database.ExecuteSqlCommand("delete from Results");
            Context.Database.ExecuteSqlCommand("delete from Questions");
            Context.Database.ExecuteSqlCommand("delete from Users");

        }

        /// <summary>
        /// Return the datetime by using datatable's time and applying user's timezone. This is used to prevent cheats where the user changes his computer's time to alter a result
        /// </summary>
        /// <returns></returns>
        public DateTime GetDBTime()
        {
            return new DateTime(Context.Database.SqlQuery<DateTime>("select GETDATE()").First().Ticks);
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
            Context.Exams.RemoveRange(Context.Exams.Where<Exam>(exam => exam.User.Id == userId));
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