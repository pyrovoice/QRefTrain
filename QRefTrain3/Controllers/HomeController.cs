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
            QuizzViewModel quizzModel = new QuizzViewModel(displayedQuestions, ResultType.Training);
            return View("Quizz", quizzModel);
        }

        [HttpPost]
        public ActionResult QuizzResult(QuizzViewModel quizzModel)
        {
            Result result = new Result() { ResultType = quizzModel.ResultType };
            List<Question> answeredQuestions = QuestionViewModelToQuestion(quizzModel.DisplayedQuestions);
            foreach (Question q in answeredQuestions)
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

        /*
         *
         *
         */
        private List<Question> QuestionViewModelToQuestion(List<QuestionQuizzViewModel> displayedQuestions)
        {
            List<Question> retrievedQuestions = new List<Question>();
            List<Question> allQuestions = Dal.Instance.getAllQuestions();
            // For each questionViewModel, we return a Question with updated Answers informations
            foreach (QuestionQuizzViewModel questionViewModel in displayedQuestions)
            {
                Question question = allQuestions.First<Question>(m => m.Id == questionViewModel.Id);
                // For each Answer, get all value answered in the viewModel and update Answers accordingly
                if(question.AnswerType == AnswerType.SingleAnswer)
                {
                    foreach(int answersIds in questionViewModel.AnswersRadio)
                    {
                        question.Answers.First<Answer>(answer => answer.Id == answersIds).IsSelected = true;
                    }
                } else if(question.AnswerType == AnswerType.MultipleAnswer)
                {
                    foreach(Answer answer in question.Answers)
                    {
                        if (questionViewModel.AnswerCheckbox.ContainsKey(answer.Id) && questionViewModel.AnswerCheckbox[answer.Id])
                        {
                            answer.IsTrue = true;
                        }
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