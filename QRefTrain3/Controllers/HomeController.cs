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
        public ActionResult Homepage()
        {
            ViewBag.ErrorQuiz = TempData["ErrorQuiz"];
            Exam exam = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name, 10);
            }
            return View(exam != null);
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
        public ActionResult MoveToQuiz(string ngb, List<string> Subjects, string NGB_Only, string isExam, string QuestionSuiteText)
        {
            // Get connected user
            User connectedUser = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                connectedUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            }

            // Check the user does not have an incoming exam (if connected)
            if(connectedUser != null && Dal.Instance.GetOngoingExamByUsername(connectedUser.Name) != null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_OngoingTest;
                return RedirectToAction("Homepage");
            }
            QuizzViewModel quizzModel;

            //========================== Start of QuestionSuite ===============================
            //If the question suite was selected, start it in exam mode
            if (!String.IsNullOrEmpty(QuestionSuiteText))
            {
                if(connectedUser == null)
                {
                    TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_LoginForTest;
                    return RedirectToAction("Homepage");
                }
                QuestionSuite suite = Dal.Instance.GetQuestionSuiteByString(QuestionSuiteText);
                if(suite == null)
                {
                    TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_NoSuiteString;
                    return RedirectToAction("Homepage");
                }
                DateTime d = Dal.Instance.GetDBTime();
                Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, suite.questions, d, suite.Id);
                quizzModel = new QuizzViewModel(suite.questions, ResultType.Exam, d, suite.Id);
                return View("Quizz", quizzModel);
            }
            //========================== End of QuestionSuite ===============================

            // Update the user's choice so he does not have to chose again next time
            Helper.CookieHelper.UpdateCookie(Request, Response, CookieNames.RequestedNGB, ngb, DateTime.Now.AddMonths(1));

            // If exam is selected, check the user is connected
            if(isExam != null && connectedUser == null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_LoginForTest;
                return RedirectToAction("Homepage");
            }

            // All verifications done, we can proceed to creating the quiz
            ResultType examType;
            List<Question> questions;
            DateTime? dt = null;
            // Depending on whether Exam or Training is selected, we set the parameters
            if (isExam != null)
            {
                examType = ResultType.Exam;
                questions = GetQuestions(ngb, null, true);
                dt = Dal.Instance.GetDBTime();
                Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, questions, dt.Value, null);
            }
            else
            {
                examType = ResultType.Training;
                questions = GetQuestions(ngb, Subjects, NGB_Only != null);
            }

            quizzModel = new QuizzViewModel(questions, examType, dt, null);
            return View("Quizz", quizzModel);
        }

        private List<Question> GetQuestions(string ngb, List<string> subjects, bool NGB_only)
        {
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = new List<Question>();
            // Get all questions, and create a list of 10 questions at random.
            allQuestions = Dal.Instance.GetQuestionsByParameter(subjects, ngb, NGB_only);

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
            Exam exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name, 10);
            if(exam == null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_TestTimeout;
                return RedirectToAction("Homepage");
            }
            return View("Quizz", new QuizzViewModel(exam.Questions, ResultType.Exam, exam.StartDate, exam.SuiteId));
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
                    quizzModel.StartTime = Dal.Instance.GetOngoingExamByUsername(currentUser.Name).StartDate;
                    // Verification : Time security
                    string LogText = null;
                    if (!quizzModel.StartTime.HasValue)
                    {
                        LogText = "Quizz result error : No start time for quizzModel.";
                    }
                    else if ((Dal.Instance.GetDBTime() - quizzModel.StartTime.Value).Minutes > 12)
                    {
                        LogText = "Quizz result error : Time between start and end of test is too high.";
                    }
                    if (LogText != null)
                    {
                        Dal.Instance.CreateLog(new Log()
                        {
                            UserId = currentUser.Id,
                            LogText = LogText,
                            LogTime = Dal.Instance.GetDBTime()
                        });
                        Dal.Instance.CloseExamByUsername(currentUser.Name);
                        return View("ErrorPage", QRefResources.Resource.Error_QuizError);
                    }
                    Dal.Instance.DeleteExamByUserId(currentUser.Id);
                }
                result.User = currentUser;
                if(quizzModel.SuiteID.HasValue)
                {
                    QuestionSuite qs = Dal.Instance.GetQuestionSuiteById(quizzModel.SuiteID.Value);
                    if(qs != null)
                    {
                        String body = "User " + currentUser.Name + " completed your exam " + qs.name + "\nResult: " + result.GetNumberGoodAnswers() + "/" + result.QuestionsAsked.Count + "\nTime: " + Dal.Instance.GetDBTime();
                        Helper.MailingHelper.SendMail(qs.owner, "User " + currentUser.Name + " completed your exam " + qs.name, body);
                        result.Reporter = qs.owner;
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