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
            Dal.Instance.CreateLog(new Log()
            {
                LogText = "User " + connectedUser != null ? connectedUser.Name : "Unknown" + "Tried to start a quiz",
                LogTime = DateTime.Now,
                User = connectedUser
            });

            // Check the user does not have an incoming exam (if connected)
            if (connectedUser != null && Dal.Instance.GetOngoingExamByUsername(connectedUser.Name) != null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_OngoingTest;
                return RedirectToAction("Homepage");
            }
            QuizzViewModel quizzModel;

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
                    quizzModel = new QuizzViewModel(ResultType.Training, Dal.Instance.GetDBTime(), suite.TimeLimit, suite.Questions);
                    return View("Quizz", quizzModel);
                }
                // Else we create an exam as usual based on the suite
                DateTime d = Dal.Instance.GetDBTime();
                Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, d, suite);
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
                Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, dt.Value, EXAM_TIME_LIMIT, questions);
                quizzModel = new QuizzViewModel(newExam);
            }
            else
            {
                examType = ResultType.Training;
                questions = GetQuestions(NGB, Subjects);
                quizzModel = new QuizzViewModel(examType, dt, EXAM_TIME_LIMIT, questions);
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
            return View("Quizz", new QuizzViewModel(ResultType.Exam, exam.StartDate, exam.Suite));
        }

        [HttpPost]
        public ActionResult CancelTest()
        {
            Dal.Instance.CloseExamByUsername(HttpContext.User.Identity.Name);
            return RedirectToAction("Homepage");
        }

        [HttpPost]
        public ActionResult QuizzResult(QuizzViewModel quizzModel)
        {
            Result result;
            result = new Result() { ResultType = quizzModel.ResultType, DateTime = Dal.Instance.GetDBTime() };
            foreach (QuestionQuizzViewModel q in quizzModel.DisplayedQuestions)
            {
                result.QuestionsAsked.Add(Dal.Instance.GetQuestionById(q.Id));
                foreach (AnswerQuizzViewModel a in q.Answers)
                {
                    if (a.IsSelected) { result.SelectedAnswers.Add(Dal.Instance.GetAnswerById(a.Id)); }
                }
            }

            // If the user is connected, save the result to his profile
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User currentUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
                // If the user took an exam, check nothing wrong happened and close the exam
                if (quizzModel.ResultType == ResultType.Exam)
                {
                    Exam exam = Dal.Instance.GetOngoingExamByUsername(currentUser.Name);
                    // Verification : Time security
                    string LogText = null;
                    if (exam.StartDate == null)
                    {
                        LogText = "Quizz result error : No start time for quizzModel.";
                    }
                    else if ((Dal.Instance.GetDBTime() - exam.StartDate).Milliseconds > exam.Suite.TimeLimit + 10000)
                    {
                        LogText = "Quizz result error : Time between start and end of test is too high.";
                    }
                    if (LogText != null)
                    {
                        Dal.Instance.CreateLog(new Log()
                        {
                            User = currentUser,
                            LogText = LogText,
                            LogTime = Dal.Instance.GetDBTime()
                        });
                        Dal.Instance.CloseExamByUsername(currentUser.Name);
                        return View("ErrorPage", QRefResources.Resource.Error_QuizError);
                    }
                    Dal.Instance.DeleteExamByUserId(currentUser.Id);
                }
                result.User = currentUser;
                if (quizzModel.Suite != null)
                {
                    QuestionSuite qs = quizzModel.Suite;
                    if (qs != null)
                    {
                        String body = "User " + currentUser.Name + " completed your exam " + qs.Name + "\nResult: " + result.GetNumberGoodAnswers() + "/" + result.QuestionsAsked.Count + "\nTime: " + Dal.Instance.GetDBTime();
                        Helper.MailingHelper.SendMail(qs.Owner, "User " + currentUser.Name + " completed your exam " + qs.Name, body);
                        result.Reporter = qs.Owner;
                    }
                }
                Dal.Instance.CreateResult(result);
            }

            return View("QuizResult", new ResultViewModel(result));
        }

        [HttpPost]
        public ActionResult UpdateLanguage(String languages)
        {
            // Validate input
            languages = CultureHelper.GetImplementedCulture(languages);
            // Save culture in a cookie
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
            return RedirectToAction("Homepage");
        }
    }
}