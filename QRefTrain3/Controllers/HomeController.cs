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
        public ActionResult MovetoTrainingQuiz(List<string> Subjects, List<string> Difficulties, string NGB, string NGB_Only)
        {
            if (Subjects == null || Difficulties == null || NGB == null)
            {
                TempData["ErrorQuizTraining"] = "Please select at least one field and difficulty";
                return RedirectToAction("Homepage");
            }
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.GetQuestionsByParameter(Subjects, Difficulties, NGB, NGB_Only != null);
            // Get 10 randoms questions from the selected parameters, or all if there is not 10.
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
            QuizzViewModel quizzModel = new QuizzViewModel(displayedQuestions, ResultType.Training);
            CookieHelper.UpdateCookie(Request, Response, CookieNames.RequestedNGB, NGB, DateTime.Now.AddHours(1));
            return View("Quizz", quizzModel);
        }

        [HttpPost]
        public ActionResult MovetoTestQuiz(string NGB)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                TempData["ErrorQuizOfficial"] = "You must be logged in in order to take an exam.";
                return RedirectToAction("Homepage");
            }
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.GetQuestionsByNGB(NGB);
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
            Helper.CookieHelper.UpdateCookie(Request, Response, CookieNames.RequestedNGB, NGB, DateTime.Now.AddHours(1));
            Exam newExam = Dal.Instance.CreateExam(HttpContext.User.Identity.Name, displayedQuestions);
            QuizzViewModel quizzModel = new QuizzViewModel(displayedQuestions, ResultType.Exam, newExam.Result.Id);
            return View("Quizz", quizzModel);

        }

        [HttpPost]
        public ActionResult ResumeTestQuiz(string NGB)
        {
            Exam exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name);
            return View("Quizz", new QuizzViewModel(Dal.Instance.GetQuestionByIds(exam.Result.QuestionsAskedIds), ResultType.Exam, exam.Result.Id));
        }

        [HttpPost]
        public ActionResult QuizzResult(QuizzViewModel quizzModel)
        {
            Result result;
            List<Question> answeredQuestions = QuestionViewModelToQuestion(quizzModel.DisplayedQuestions);
            // If the result already exists in db, we get it back and check that the questions are the same
            if (quizzModel.ResultId != null)
            {
                result = Dal.Instance.GetResultById(quizzModel.ResultId.Value);
                bool isErrorDifferenceQuestion = false;
                foreach (Question q in answeredQuestions)
                {
                    if (!result.QuestionsAskedIds.Contains(q.Id))
                    {
                        isErrorDifferenceQuestion = true;
                        break;
                    }
                }
                if (isErrorDifferenceQuestion)
                {
                    string logText = "Error when comparing live test questions to remembered test questions.\nDB Questions IDs : ";
                    foreach (int id in result.QuestionsAskedIds)
                    {
                        logText += id + " ";
                    }
                    logText += "\nLive questions IDs : ";
                    foreach (Question q in answeredQuestions)
                    {
                        logText += q.Id + " ";
                    }
                    Dal.Instance.CreateLog(new Log()
                    {
                        LogTime = DateTime.Now,
                        UserId = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name).Id,
                        LogText = logText
                    });
                    TempData["ErrorQuizOfficial"] = "There was an error when resolving this test. Please send us a mail if this happens again.";
                    return RedirectToAction("Homepage");
                }
            }
            // Otherwise, we create a new result and populate it with the questions
            else
            {
                result = new Result() { ResultType = quizzModel.ResultType, DateTime = DateTime.Now };
                foreach(Question q in answeredQuestions)
                {
                    result.QuestionsAskedIds.Add(q.Id);
                }
            }
            // Then Create the result with the answers selected by the user
            foreach (Question q in answeredQuestions)
            {
                foreach (Answer a in q.Answers)
                {
                    if (a.IsSelected)
                    {
                        result.SelectedAnswers.Add(a.Id);
                    }
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                result.User = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
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

        private List<Question> QuestionViewModelToQuestion(List<QuestionQuizzViewModel> displayedQuestions)
        {
            List<Question> retrievedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.getAllQuestions();
            // For each questionViewModel, we return a Question with updated Answers informations
            foreach (QuestionQuizzViewModel questionViewModel in displayedQuestions)
            {
                Question question = allQuestions.First<Question>(m => m.Id == questionViewModel.Id);
                if (question.AnswerType == AnswerType.MultipleAnswer)
                {
                    foreach (Answer answer in question.Answers)
                    {
                        answer.IsSelected = questionViewModel.Answers.First<AnswerQuizzViewModel>(m => m.Id == answer.Id).IsSelected;
                    }
                }
                retrievedQuestions.Add(question);
            }
            return retrievedQuestions;
        }

        public ActionResult QuestionListing()
        {
            List<Question> allQuestions = allQuestions = Dal.Instance.getAllQuestions();
            return View("QuestionListing", allQuestions);
        }
    }
}