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
            ViewBag.ErrorQuizTraining = TempData["ErrorQuizTraining"];
            ViewBag.ErrorQuizOfficial = TempData["ErrorQuizOfficial"];
            return View(HttpContext.User.Identity.IsAuthenticated && Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name) != null);
        }

        /// <summary>
        /// Load questions according to parameters, then load a quizz page with those questions
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public ActionResult MovetoTrainingQuiz(List<string> Subjects, string NGB, string NGB_Only)
        {
            if (Subjects == null || NGB == null)
            {
                TempData["ErrorQuizTraining"] = "Please select at least one subject";
                return RedirectToAction("Homepage");
            }
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.GetQuestionsByParameter(Subjects, NGB, NGB_Only != null);
            // Get 10 randoms questions from the selected parameters, or all if there is not 10.
            if (allQuestions.Count <= 0)
            {
                TempData["ErrorQuizTraining"] = "There is no question for the selected subject(s)";
                return RedirectToAction("Homepage");
            }
            else if (allQuestions.Count < 10)
            {
                displayedQuestions = allQuestions;
            }
            else
            {
                Random rnd = new Random();
                while (displayedQuestions.Count < 10)
                {
                    int random = rnd.Next(allQuestions.Count - 1);
                    Question question = allQuestions[random];
                    if (!displayedQuestions.Contains(question))
                    {
                        displayedQuestions.Add(question);
                    }
                }
            }
            QuizzViewModel quizzModel = new QuizzViewModel(displayedQuestions, ResultType.Training, null);
            CookieHelper.UpdateCookie(Request, Response, CookieNames.RequestedNGB, NGB, DateTime.Now.AddYears(1));
            return View("Quizz", quizzModel);
        }

        [HttpPost]
        public ActionResult MovetoTestQuiz(string NGB)
        {
            // Update the user's choice so he does not have to chose again next time
            Helper.CookieHelper.UpdateCookie(Request, Response, CookieNames.RequestedNGB, NGB, DateTime.Now.AddYears(1));
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                TempData["ErrorQuizOfficial"] = "You must be logged in in order to take an exam.";
                return RedirectToAction("Homepage");
            }
            // In case the user refresh the page, do not recreate a form
            Exam exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name);
            if (exam != null)
            {
                return View("Quizz", new QuizzViewModel(exam.Questions, ResultType.Exam, exam.StartDate));
            }
            else
            {
                Dal.Instance.CloseExamByUsername(HttpContext.User.Identity.Name);
            }
            // Get all questions, and create a list of 10 questions at random.
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.GetQuestionsByNGB(NGB);
            if (allQuestions.Count <= 0)
            {
                TempData["ErrorQuizTraining"] = "There is no question for the selected subject(s)";
                return RedirectToAction("Homepage");
            }
            else if (allQuestions.Count < 10)
            {
                displayedQuestions = allQuestions;
            }
            else
            {
                Random rnd = new Random();
                while (displayedQuestions.Count < 10)
                {
                    int random = rnd.Next(allQuestions.Count - 1);
                    Question question = allQuestions[random];
                    if (!displayedQuestions.Contains(question))
                    {
                        displayedQuestions.Add(question);
                    }
                }
            }
            // Create an exam object that will remember the user is doing na exam, which prevent cheating and allow the user to resume a test in case of error
            DateTime timenow = Dal.Instance.GetDBTime();
            Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, displayedQuestions, timenow);
            QuizzViewModel quizzModel = new QuizzViewModel(displayedQuestions, ResultType.Exam, timenow);
            return View("Quizz", quizzModel);

        }

        [HttpPost]
        public ActionResult ResumeTestQuiz()
        {
            Exam exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name, 10);
            return View("Quizz", new QuizzViewModel(exam.Questions, ResultType.Exam, exam.StartDate));
        }

        [HttpPost]
        public ActionResult QuizzResult(QuizzViewModel quizzModel)
        {
            Result result;
            result = new Result() { ResultType = quizzModel.ResultType, DateTime = Dal.Instance.GetDBTime() };
            foreach (QuestionQuizzViewModel q in quizzModel.DisplayedQuestions)
            {
                result.QuestionsAsked.Add(Dal.Instance.GetQuestionById(q.Id));
                foreach(AnswerQuizzViewModel a in q.Answers)
                {
                    if (a.IsSelected) { result.SelectedAnswers.Add(Dal.Instance.GetAnswerById(a.Id)); }
                }
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User currentUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
                if (quizzModel.ResultType == ResultType.Exam)
                {
                    quizzModel.StartTime = Dal.Instance.GetOngoingExamByUsername(currentUser.Name).StartDate;
                    if (!quizzModel.StartTime.HasValue)
                    {
                        Dal.Instance.CreateLog(new Log()
                        {
                            UserId = currentUser.Id,
                            LogText = "Quizz result error : No start time for quizzModel.",
                            LogTime = Dal.Instance.GetDBTime()
                        });
                        Dal.Instance.CloseExamByUsername(currentUser.Name);
                        return View("ErrorPage", QRefResources.Resource.Error_QuizError);
                    }
                    if ((Dal.Instance.GetDBTime() - quizzModel.StartTime.Value).Minutes > 12)
                    {
                        Dal.Instance.CreateLog(new Log()
                        {
                            UserId = currentUser.Id,
                            LogText = "Quizz result error : Time between start and end of test is too high.",
                            LogTime = Dal.Instance.GetDBTime()
                        });
                        Dal.Instance.CloseExamByUsername(currentUser.Name);
                        return View("ErrorPage", QRefResources.Resource.Error_QuizError);
                    }

                    Dal.Instance.DeleteExamByUserId(currentUser.Id);
                }
                result.User = currentUser;
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