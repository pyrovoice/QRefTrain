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
        public AnswerType AnswerType { get; set; }
        public List<AnswerQuizzViewModel> Answers { get; set; }
        
        // Lists containing user's answers. They have to be separated because RadioButtonFor and CheckBoxFor are incompatible when used together

        // Use this to store checkbox answers : Boolean switch. Key is answer's ID, value is True/false (selected or not)
        public Dictionary<int, Boolean> AnswerCheckbox { get; set; }
        // Use this to store radioButton answers : All radioButtons register to the same list, thus being in the same group
        // Key is Question's ID, value is selected Answers' Id
        public List<int> AnswersRadio { get; set; }

        public QuestionQuizzViewModel(int id, string name, bool isVideo, string videoURL, string questionText, AnswerType answerType, List<AnswerQuizzViewModel> answers)
        {
            Id = id;
            Name = name;
            IsVideo = isVideo;
            VideoURL = videoURL;
            QuestionText = questionText;
            AnswerType = answerType;
            Answers = answers;
            // Initialize the list used in the viewModel, depending on the answer type. The other won't be used
            if (answerType == AnswerType.SingleAnswer)
            {
                AnswersRadio = new List<int>();
            }
            else if (answerType == AnswerType.MultipleAnswer)
            {
                AnswerCheckbox = new Dictionary<int, bool>();
                foreach (AnswerQuizzViewModel a in answers)
                {
                    AnswerCheckbox.Add(a.Id, false);
                }
            }
        }

        public QuestionQuizzViewModel(Question question)
        {
            List<AnswerQuizzViewModel> answersViewModel = new List<AnswerQuizzViewModel>();
            foreach (Answer a in question.Answers)
            {
                answersViewModel.Add(new AnswerQuizzViewModel
                {
                    Id = a.Id,
                    AnswerText = a.Answertext

                });
            }
            Id = question.Id;
            Name = question.Name;
            IsVideo = question.IsVideo;
            VideoURL = question.VideoURL;
            QuestionText = question.QuestionText;
            AnswerType = question.AnswerType;
            Answers = answersViewModel;
            // Initialize the list used in the viewModel, depending on the answer type. The other won't be used
            if(question.AnswerType == AnswerType.SingleAnswer)
            {
                AnswersRadio = new List<int>();
            } else if (question.AnswerType == AnswerType.MultipleAnswer)
            {
                AnswerCheckbox = new Dictionary<int, bool>();
                foreach(AnswerQuizzViewModel a in answersViewModel)
                {
                    AnswerCheckbox.Add(a.Id, false);
                }
            }
        }

        public QuestionQuizzViewModel()
        {

        }
    }
}