using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class QuizzViewModel
    {
        public List<QuestionQuizzViewModel> DisplayedQuestions { get; set; }
        public ResultType ResultType { get; set; }

        public QuizzViewModel(List<Question> displayedQuestions, ResultType type)
        {
            List<QuestionQuizzViewModel> questionsViewModel = new List<QuestionQuizzViewModel>();
            foreach(Question question in displayedQuestions)
            {
                questionsViewModel.Add(QuestionToViewModel(question));

            }
            this.ResultType = type;
        }

        private static QuestionQuizzViewModel QuestionToViewModel(Question question)
        {
            
            List<AnswerQuizzViewModel> answersViewModel = new List<AnswerQuizzViewModel>();
            foreach(Answer a in question.Answers)
            {
                answersViewModel.Add(new AnswerQuizzViewModel
                {
                    Id = a.Id,
                    AnswerText = a.Answertext

                });
            }
            QuestionQuizzViewModel questionViewModel = new QuestionQuizzViewModel
            {
                Answers = answersViewModel,
                IsVideo = question.IsVideo,
                VideoURL = question.VideoURL,
                AnswerType = question.AnswerType,
                Id = question.Id,
                Name = question.Name,
                QuestionText = question.QuestionText,
                AnswerCheckbox = new Dictionary<int, bool>(),
                AnswersRadio = new Dictionary<int, List<int>>()
            };
            return questionViewModel;
        }
    }
}