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
    public class HomeController : BaseController
    {
        //2mn30
        private static readonly int EXAM_TIME_LIMIT = 1500000;
        private static readonly int EXAM_NBR_QUESTIONS = 10;

        public ActionResult Homepage(String suiteID)
        {
            if (!String.IsNullOrEmpty(suiteID))
            {
                return MoveToQuiz(null, null, null, suiteID);
            }
            ViewBag.ErrorQuiz = TempData["ErrorQuiz"];
            Exam exam = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name);
            }
            return View(exam != null);
        }

        [HttpPost]
        public ActionResult MovetoExam(string NGB_Exam)
        {
            if (!String.IsNullOrEmpty(NGB_Exam))
            {
                return MoveToQuiz(NGB_Exam, null, true, null);
            }
            TempData["ErrorQuiz"] = QRefResources.Resource.Error_SelectNGB;
            return RedirectToAction("Homepage");
        }

        /// <summary>
        /// Check the user is not doing wrong manipulations, then loads questions and the quiz page according to the input parameters
        /// </summary>
        /// <param name="ngb"></param>
        /// <param name="Subjects"></param>
        /// <param name="NGB_Only"></param>
        /// <param name="isExam"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MoveToQuiz(string NGB, List<string> Subjects, bool? isExam, string QuestionSuiteText)
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
            QuizViewModel quizzModel;

            //========================== Start of QuestionSuite ===============================
            //If the question suite was selected, start it in exam mode
            if (!String.IsNullOrEmpty(QuestionSuiteText))
            {
                QuestionSuite suite = Dal.Instance.GetQuestionSuiteByString(QuestionSuiteText);
                if (suite == null)
                {
                    TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_NoSuiteString;
                    return RedirectToAction("Homepage");
                }
                //If user is not connected, we cannot register that the exam started froma  suite. Then, we consider it a training quiz, allowing a user to still take the suite while not being registered
                if (connectedUser == null)
                {
                    quizzModel = new QuizViewModel(ResultType.Training, Dal.Instance.GetDBTime(), suite);
                    return View("Quizz", quizzModel);
                }
                // Else we create an exam as usual based on the suite
                DateTime d = Dal.Instance.GetDBTime();
                Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, d, suite);
                quizzModel = new QuizViewModel(newExam);
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
                Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, dt.Value, EXAM_TIME_LIMIT, questions);
                quizzModel = new QuizViewModel(newExam);
            }
            else
            {
                examType = ResultType.Training;
                questions = GetQuestions(NGB, Subjects);
                quizzModel = new QuizViewModel(examType, dt, EXAM_TIME_LIMIT, null);
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

        [HttpPost]
        public ActionResult ResumeTestQuiz()
        {
            Exam exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name);
            if (exam == null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_TestTimeout;
                return RedirectToAction("Homepage");
            }
            return View("Quizz", new QuizViewModel(ResultType.Exam, exam.StartDate, exam.Suite));
        }

        [HttpPost]
        public ActionResult CancelTest()
        {
            Dal.Instance.CloseExamByUsername(HttpContext.User.Identity.Name);
            return RedirectToAction("Homepage");
        }

        [HttpPost]
        public ActionResult QuizzResult(QuizViewModel quizzModel)
        {
            Result result = ResultFromQuizModel(quizzModel);
            new QuizResolver(result).ValidateAndResolveQuizResult();
            return View("QuizResult", new ResultViewModel(result));
        }

        private Result ResultFromQuizModel(QuizViewModel quizModel)
        {
            List<Question> questionsAsked = new List<Question>();
            List<Answer> answers = new List<Answer>();
            // As questions and answers are managed differently in the quiz, we need to retrieve questions and all checked answers.
            foreach (QuestionQuizzViewModel q in quizModel.DisplayedQuestions)
            {
                questionsAsked.Add(Dal.Instance.GetQuestionById(q.Id));
                foreach (AnswerQuizzViewModel a in q.Answers)
                {
                    if (a.IsSelected) { answers.Add(Dal.Instance.GetAnswerById(a.Id)); }
                }
            }
            Result result = new Result(Dal.Instance.GetUserByName(HttpContext.User.Identity.Name), 
                questionsAsked, answers,
                quizModel.ResultType, 
                Dal.Instance.GetDBTime(), 
                quizModel.Suite);

            return result;
        }

        [HttpPost]
        public ActionResult UpdateLanguage(String userSelectedLanguage)
        {
            userSelectedLanguage = CultureHelper.GetImplementedOrDefaultCulture(userSelectedLanguage);
            CookieHelper.UpdateCookie(Request, Response, CookieNames.languageCookie, userSelectedLanguage, DateTime.Now.AddYears(1));
            /*
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = languages;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture")
                {
                    Value = languages,
                    Expires = DateTime.Now.AddYears(1)
                };
            }
            Response.Cookies.Add(cookie);
            */
            return RedirectToAction("Homepage");
        }
    }
}