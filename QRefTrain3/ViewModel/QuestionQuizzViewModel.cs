using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class QuestionQuizzViewModel
    {
        public int Id { get; set; }
        public string GifName { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerQuizzViewModel> Answers { get; set; }
        public int SelectedAnswerId { get; set; }
        public string PublicId { get; set; }

        public QuestionQuizzViewModel(Question question)
        {
            List<AnswerQuizzViewModel> answersViewModel = new List<AnswerQuizzViewModel>();
            foreach (Answer a in question.Answers)
            {
                answersViewModel.Add(new AnswerQuizzViewModel
                {
                    Id = a.Id,
                    AnswerText = a.Answertext,
                    IsSelected = false
                });
            }
            Id = question.Id;
            GifName = question.GifName;
            QuestionText = question.QuestionText;
            Answers = answersViewModel;
            this.PublicId = question.PublicId;
        }

        public QuestionQuizzViewModel()
        {

        }
    }
}