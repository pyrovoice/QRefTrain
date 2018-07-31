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
        public ActionResult MovetoQuiz(List<string> Subjects, List<string> Difficulties, string NGBs)
        {
            List<Question> displayedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.GetQuestionsByParameter(Subjects, Difficulties, NationalGoverningBody.All);
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
            return View("Quizz", quizzModel);
        }

        [HttpPost]
        public ActionResult QuizzResult(QuizzViewModel quizzModel)
        {
            Result result = new Result() { ResultType = quizzModel.ResultType, DateTime = DateTime.Now};
            List<Question> answeredQuestions = QuestionViewModelToQuestion(quizzModel.DisplayedQuestions);
            foreach (Question q in answeredQuestions)
            {
                result.QuestionsAskedIds.Add(q.Id);
                foreach(Answer a in q.Answers)
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

        private List<Question> QuestionViewModelToQuestion(List<QuestionQuizzViewModel> displayedQuestions)
        {
            List<Question> retrievedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.getAllQuestions();
            // For each questionViewModel, we return a Question with updated Answers informations
            foreach (QuestionQuizzViewModel questionViewModel in displayedQuestions)
            {
                Question question = allQuestions.First<Question>(m => m.Id == questionViewModel.Id);
                if(question.AnswerType == AnswerType.MultipleAnswer)
                {
                    foreach(Answer answer in question.Answers)
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