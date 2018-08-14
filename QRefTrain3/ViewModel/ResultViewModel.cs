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
        public List<int> SelectedAnswers { get; set; } = new List<int>();
        public ResultType ResultType { get; set; }
        public DateTime DateTime { get; set; }

        public ResultViewModel(Result result)
        {
            this.Id = result.Id;
            // Order by to keep the same order displayed in the quiz and result
            this.QuestionsAsked = Dal.Instance.GetQuestionByIds(result.QuestionsAskedIds).OrderBy(d => result.QuestionsAskedIds.IndexOf(d.Id)).ToList();
            this.SelectedAnswers = result.SelectedAnswers;
            this.ResultType = result.ResultType;
            this.DateTime = result.DateTime;
        }

        public int GetNumberGoodAnswer()
        {
            int goodAnswers = 0;
            foreach (Question q in QuestionsAsked)
            {
                if (Question.IsQuestionCorrect(q, SelectedAnswers))
                {
                    goodAnswers++;
                }
            }
            return goodAnswers;
        }
    }
}