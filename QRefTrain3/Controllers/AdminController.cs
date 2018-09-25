using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRefTrain3.Controllers
{
    public class AdminController : BaseController
    {

        public ActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Homepage", "Home");
            }
            /*
            User user = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            if (user.UserRole != UserRole.Admin)
            {
                Dal.Instance.CreateLog(new Log()
                {
                    LogText = "User tried to access admin page without right. User : " + user.Name + ", Right : " + user.UserRole,
                    LogTime = DateTime.Now,
                    UserId = user.Id

                });
                return RedirectToAction("Homepage", "Home");
            }*/
            return View();
        }

        public ActionResult Export()
        {
            string test = "";
            string delimiter = ";";
            foreach(Question q in Dal.Instance.getAllQuestions())
            {
                test += q.QuestionText + delimiter + q.AnswerExplanation + delimiter + q.Subject + delimiter + q.VideoURL + delimiter + q.NationalGoverningBodies + delimiter;
                foreach(Answer a in q.Answers)
                {
                    test += a.Answertext + delimiter + a.IsTrue + delimiter;
                }
                test += "\n";
            }

            System.IO.File.WriteAllText("C:\\Users\\maxim\\Desktop\\data.txt", test);

            return View();
        }

        public ActionResult QuestionListing()
        {
            var questions = Dal.Instance.getAllQuestions();
            foreach (Question q in questions)
            {
                q.Answers.Sort((val1, val2) => val1.CompareTo(val2));
            }
            return View("QuestionsListing", questions);
        }

        [HttpPost]
        public ActionResult AddQuestion(string BaseName, bool isNoPenalty, bool isTurnOver, bool isBlueCard, bool isYellowCard, bool isRedCard, bool isBackToHoops,
            bool isNoPenaltyTrue, bool isTurnOverTrue, bool isBlueCardTrue, bool isYellowCardTrue, bool isRedCardTrue, bool isBackToHoopsTrue,
            bool isAnswer1, bool isAnswer2, bool isAnswer3, bool isAnswer4, bool isAnswer5, bool isAnswer6,
            bool isAnswerTrue1, bool isAnswerTrue2, bool isAnswerTrue3, bool isAnswerTrue4, bool isAnswerTrue5, bool isAnswerTrue6,
            string Video, string QuestionSubject, List<string> NGBs)
        {
            BaseName = "Question" + BaseName;
            List<Answer> answers = new List<Answer>();
            if (isAnswer1) { answers.Add(new Answer(BaseName + "Answer1", isAnswerTrue1)); }
            if (isAnswer2) { answers.Add(new Answer(BaseName + "Answer2", isAnswerTrue2)); }
            if (isAnswer3) { answers.Add(new Answer(BaseName + "Answer3", isAnswerTrue3)); }
            if (isAnswer4) { answers.Add(new Answer(BaseName + "Answer4", isAnswerTrue4)); }
            if (isAnswer5) { answers.Add(new Answer(BaseName + "Answer5", isAnswerTrue5)); }
            if (isAnswer6) { answers.Add(new Answer(BaseName + "Answer6", isAnswerTrue6)); }
            if (isNoPenalty) {
                answers.Add(new Answer("NoPenalty", isNoPenaltyTrue));
            }
            if (isTurnOver) {
                answers.Add(new Answer("Turnover", isTurnOverTrue));
            }
            if (isBlueCard) {
                answers.Add(new Answer("BlueCard", isBlueCardTrue));
            }
            if (isYellowCard) {
                answers.Add(new Answer("YellowCard", isYellowCardTrue));
            }
            if (isRedCard) {
                answers.Add(new Answer("RedCard", isRedCardTrue));
            }
            if (isBackToHoops)
            {
                answers.Add(new Answer("backToHoops", isBackToHoopsTrue));
            }

            Question questionToAdd = new Question(BaseName + "Title", (Models.QuestionSubject)Enum.Parse(typeof(Models.QuestionSubject), QuestionSubject), Video, BaseName + "Text", answers, BaseName + "Explanation", NGBs.ToArray());
            Dal.Instance.CreateQuestion(questionToAdd);
            return RedirectToAction("Index");
        }
    }
}