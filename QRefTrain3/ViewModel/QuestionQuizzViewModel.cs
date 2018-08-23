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
        public string Name { get; set; }
        public bool IsVideo { get; set; }
        public string VideoURL { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerQuizzViewModel> Answers { get; set; }

        public QuestionQuizzViewModel(int id, string name, bool isVideo, string videoURL, string questionText, List<AnswerQuizzViewModel> answers)
        {
            Id = id;
            Name = name;
            IsVideo = isVideo;
            VideoURL = videoURL;
            QuestionText = questionText;
            Answers = answers;
        }

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
            Name = question.Name;
            IsVideo = question.IsVideo;
            VideoURL = question.VideoURL;
            QuestionText = question.QuestionText;
            Answers = answersViewModel;
        }

        public QuestionQuizzViewModel()
        {

        }
    }
}