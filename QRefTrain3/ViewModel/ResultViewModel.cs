using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class ResultViewModel
    {
        public int Id { get; set; }
        public List<Question> QuestionsAsked { get; set; } = new List<Question>();
        public List<Answer> SelectedAnswers { get; set; } = new List<Answer>();
        public QuizType ResultType { get; set; }
        public DateTime DateTime { get; set; }

        

        public ResultViewModel(Result result)
        {
            this.Id = result.Id;
            // Order by to keep the same order displayed in the quiz and result
            this.QuestionsAsked = result.QuestionsAsked.OrderBy(d => result.QuestionsAsked.IndexOf(d)).ToList();
            this.SelectedAnswers = result.SelectedAnswers;
            this.ResultType = result.QuizType;
            this.DateTime = result.DateTime;
        }

        public int GetNumberGoodAnswer()
        {
            int goodAnswers = 0;
            foreach (Question q in QuestionsAsked)
            {
                if (q.IsQuestionCorrect(SelectedAnswers))
                {
                    goodAnswers++;
                }
            }
            return goodAnswers;
        }

        public bool IsResultSuccesfull()
        {
            return ((float)this.GetNumberGoodAnswer()) / ((float)this.QuestionsAsked.Count()) >= 0.8;
        }
    }
}