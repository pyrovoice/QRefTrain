using QRefTrain3.Models;
using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRefTrain3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Homepage()
        {
            return View();
        }

        /// <summary>
        /// Load questions according to parameters, then load a quizz page with those questions
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public ActionResult MovetoQuiz(string QuizzField, string QuizzDifficulty)
        {
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.GetQuestionsByParameter(QuizzField, QuizzDifficulty);
            // In case we don't have enough questions
            if (allQuestions.Count < 10)
            {
                displayedQuestions = allQuestions;
            }
            else
            {
                Random rnd = new Random();
                while (displayedQuestions.Count < 10)
                {
                    Question question = allQuestions[rnd.Next(allQuestions.Count - 1)];
                    if (!displayedQuestions.Contains(question))
                    {
                        displayedQuestions.Add(allQuestions[rnd.Next(allQuestions.Count - 1)]);
                    }
                }
            }
            QuizzViewModel quizzModel =  new QuizzViewModel() { DisplayedQuestions = displayedQuestions, ResultType = ResultType.Training };
            return View("Quizz", quizzModel);
        }

        [HttpPost]
        public ActionResult QuizzResult(QuizzViewModel quizzModel)
        {
            Result result = new Result() { ResultType = quizzModel.ResultType };
            foreach (Question q in quizzModel.DisplayedQuestions)
            {
                result.QuestionsAskedIds.Add(q.Id);
                if (Question.IsGoodAnswer(q))
                {
                    result.GoodAnswersIds.Add(q.Id);
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                result.User = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
                Dal.Instance.CreateResult(result);
            }

            return View("QuizResult", result);
        }

        public ActionResult QuestionListing()
        {
            List<Question> allQuestions = allQuestions = Dal.Instance.getAllQuestions();
            return View("QuestionListing", allQuestions);
        }
    }
}