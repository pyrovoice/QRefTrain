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
        //1 hour
        private static readonly int EXAM_TIME_LIMIT = 1500000;
        private static readonly int EXAM_NBR_QUESTIONS = 10;

        public ActionResult Quiz(String quizTemplateCode)
        {
            if (!String.IsNullOrWhiteSpace(quizTemplateCode))
            {
                return StartQuizFromUserQuizSuite(quizTemplateCode);
            }
            return StartTrainingQuiz(NationalGoverningBody.All.ToString(), null);
        }

        [HttpPost]
        public ActionResult StartTestQuiz(string NGB)
        {
            if (!CheckConditionForTestQuizOK(NGB))
            {
                return RedirectToAction("Homepage", "Home");
            }

            Quiz quiz = Dal.Instance.CreateQuiz(HttpContext.User.Identity.Name, EXAM_TIME_LIMIT, GetQuestions(NGB, EXAM_NBR_QUESTIONS));
            QuizViewModel quizModel = new QuizViewModel(quiz);
            return View("Quiz", quizModel);
        }

        private bool CheckConditionForTestQuizOK(string ngb)
        {

            User connectedUser = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                connectedUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            }
            if (connectedUser == null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_LoginForTest;
                return false;
            }

            Quiz exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name);
            if (exam != null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_OngoingTest;
                return false;
            }

            return CheckConditionForAllQuizTypeOk(ngb, null);
        }

        [HttpPost]
        public ActionResult StartTrainingQuiz(string NGB, List<string> subjects)
        {
            if (!CheckConditionForTrainingQuizOK(NGB, subjects))
            {
                return RedirectToAction("Homepage", "Home");
            }

            QuizViewModel quizzModel = new QuizViewModel(QuizType.Training, GetQuestions(NGB, EXAM_NBR_QUESTIONS));
            return View("Quiz", quizzModel);
        }

        private bool CheckConditionForTrainingQuizOK(string ngb, List<String> subjects)
        {
            return CheckConditionForAllQuizTypeOk(ngb, subjects);
        }

        [HttpPost]
        public ActionResult StartQuizFromUserQuizSuite(string questionSuiteText)
        {
            if (!CheckConditionForQuizFromTemplateOK(questionSuiteText))
            {
                return RedirectToAction("Homepage", "Home");
            }
            QuizTemplate quizTemplate = Dal.Instance.GetQuestionSuiteByString(questionSuiteText);
            QuizViewModel quizModel = new QuizViewModel(QuizType.Training, Dal.Instance.GetDBTime(), quizTemplate);
            return View("Quiz", quizModel);
        }

        private bool CheckConditionForQuizFromTemplateOK(string questionSuiteText)
        {
            if (String.IsNullOrWhiteSpace(questionSuiteText))
            {
                TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_NoSuiteString;
                return false;
            }
            QuizTemplate suite = Dal.Instance.GetQuestionSuiteByString(questionSuiteText);
            if (suite == null)
            {
                TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_NoSuiteString;
                return false;
            }

            if (suite.Owner == null)
            {
                TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_QuizTemplateOwnerDoesNotExist;
                return false;

            }
            if(suite.Questions.Count <= 0)
            {
                TempData["ErrorQuestionSuite"] = QRefResources.Resource.Error_QuizTemplateContainsNoQuestion;
                return false;
            }
            return true;
        }

        private bool CheckConditionForAllQuizTypeOk(string ngb, List<String> subjects)
        {
            if (String.IsNullOrWhiteSpace(ngb))
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_SelectNGB;
                return false;
            }

            List<Question> questionsAvailable = Dal.Instance.GetQuestionsByParameter(ngb, subjects);
            if (questionsAvailable.Count <= 0)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_NoQuestionsForTest;
                return false;
            }
            return true;
        }

        private List<Question> GetQuestions(string ngb, int numberQuestionsToSelect)
        {
            return GetQuestions(ngb, numberQuestionsToSelect, null);
        }

        private List<Question> GetQuestions(string ngb, int numberQuestionsToSelect, List<string> subjects)
        {
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = new List<Question>();

            allQuestions = Dal.Instance.GetQuestionsByParameter(ngb, subjects);
            displayedQuestions = SelectQuestionsFromListAtRandom(allQuestions, numberQuestionsToSelect);
            SortAnswersInQuestionlist(displayedQuestions);

            return displayedQuestions;
        }

        private List<Question> SelectQuestionsFromListAtRandom(List<Question> questionsToSelectFrom, int numberQuestionsToSelect)
        {
            // We do some shuffle to randomly select and display the questions
            Random rnd = new Random();
            List<Question> selectedQuestions;
            if (questionsToSelectFrom.Count <= numberQuestionsToSelect)
            {
                selectedQuestions = questionsToSelectFrom.OrderBy(item => rnd.Next()).ToList();
            }
            else
            {
                selectedQuestions = questionsToSelectFrom.OrderBy(item => rnd.Next()).ToList().GetRange(0, numberQuestionsToSelect);
            }
            return selectedQuestions;
        }

        private void SortAnswersInQuestionlist(List<Question> displayedQuestions)
        {
            foreach (Question q in displayedQuestions)
            {
                q.Answers.Sort((val1, val2) => val1.CompareTo(val2));
            }
        }

        [HttpPost]
        public ActionResult ResumeTestQuiz()
        {
            Quiz exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name);
            if (exam == null)
            {
                TempData["ErrorQuiz"] = QRefResources.Resource.Error_TestTimeout;
                return RedirectToAction("Homepage", "Home");
            }
            return View("Quiz", new QuizViewModel(QuizType.Exam, exam.StartDate, exam.Suite));
        }

        [HttpPost]
        public ActionResult CancelTest()
        {
            Dal.Instance.CloseExamByUsername(HttpContext.User.Identity.Name);
            return RedirectToAction("Homepage");
        }

        [HttpPost]
        public ActionResult QuizResult(QuizViewModel quizModel)
        {
            Result result = ResultFromQuizModel(quizModel);
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
            QuizTemplate questionSuite = null;
            if (quizModel.QuestionSuiteId.HasValue)
            {
                questionSuite = Dal.Instance.GetQuestionSuiteById(quizModel.QuestionSuiteId.Value);
            }


            Result result = new Result(Dal.Instance.GetUserByName(HttpContext.User.Identity.Name),
                questionsAsked, answers,
                quizModel.ResultType,
                Dal.Instance.GetDBTime(),
                questionSuite);

            return result;
        }


    }
}