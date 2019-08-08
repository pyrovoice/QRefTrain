using QRefTrain3.Helper;
using QRefTrain3.Models;
using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRefTrain3.Controllers
{
    public class QuizController : Controller
    {
        //2mn30
        private static readonly int EXAM_TIME_LIMIT = 1500000;
        private static readonly int EXAM_NBR_QUESTIONS = 10;

        // GET: Quiz
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Check the user is not doing wrong manipulations, then loads questions and the quiz page according to the input parameters
        /// </summary>
        /// <param name="ngb"></param>
        /// <param name="Subjects"></param>
        /// <param name="NGB_Only"></param>
        /// <param name="isExam"></param>
        /// <returns></returns>
        public ActionResult MoveToQuiz(string NGB, List<string> Subjects, bool? isExam, string suiteID)
        {
            // Get connected user
            User connectedUser = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                connectedUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            }

            // Check the user does not have an incoming exam (if connected)
            if (connectedUser != null && Dal.Instance.GetOngoingExamByUsername(connectedUser.Name) != null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_OngoingTest;
                return RedirectToAction("Homepage");
            }
            QuizzViewModel quizzModel;

            //========================== Start of QuestionSuite ===============================
            //If the question suite was selected, start it in exam mode
            if (!String.IsNullOrEmpty(suiteID))
            {
                if (connectedUser == null)
                {
                    TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_LoginForTest;
                    return RedirectToAction("Homepage");
                }
                QuestionSuite suite = Dal.Instance.GetQuestionSuiteByString(suiteID);
                if (suite == null)
                {
                    TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_NoSuiteString;
                    return RedirectToAction("Homepage");
                }
                DateTime d = Dal.Instance.GetDBTime();
                Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, suite.Questions, d, suite.TimeLimit, suite.Id);
                quizzModel = new QuizzViewModel(newExam);
                return View("Quizz", quizzModel);
            }
            //========================== End of QuestionSuite ===============================

            // Update the user's choice so he does not have to chose again next time
            Helper.CookieHelper.UpdateCookie(Request, Response, CookieNames.RequestedNGB, NGB, DateTime.Now.AddMonths(1));

            // If exam is selected, check the user is connected
            if (isExam.HasValue && isExam.Value && connectedUser == null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_LoginForTest;
                return RedirectToAction("Homepage");
            }

            // All verifications done, we can proceed to creating the quiz
            ResultType examType;
            List<Question> questions;
            DateTime? dt = null;
            // Depending on whether Exam or Training is selected, we set the parameters
            if (isExam.HasValue && isExam.Value == true)
            {
                examType = ResultType.Exam;
                questions = GetQuestions(NGB, null);
                dt = Dal.Instance.GetDBTime();
                Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, questions, dt.Value, EXAM_TIME_LIMIT, null);
                quizzModel = new QuizzViewModel(newExam);
            }
            else
            {
                examType = ResultType.Training;
                questions = GetQuestions(NGB, Subjects);
                quizzModel = new QuizzViewModel(questions, examType, dt, EXAM_TIME_LIMIT, null);
            }

            return View("Quizz", quizzModel);
        }

        private List<Question> GetQuestions(string ngb, List<string> subjects)
        {
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = new List<Question>();
            // Get all questions, and create a list of 10 questions at random.
            allQuestions = Dal.Instance.GetQuestionsByParameter(subjects, ngb);

            if (allQuestions.Count <= 0)
            {
                return null;
            }
            else
            {
                // We do some shuffle to randomly select and display the questions
                Random rnd = new Random();
                displayedQuestions = allQuestions.Count > 10 ? allQuestions.OrderBy(item => rnd.Next()).ToList().GetRange(0, 10) : allQuestions.OrderBy(item => rnd.Next()).ToList();
            }
            foreach (Question q in displayedQuestions)
            {
                q.Answers.Sort((val1, val2) => val1.CompareTo(val2));
            }
            return displayedQuestions;
        }
    }
}