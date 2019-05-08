using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class Result
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<Question> QuestionsAsked { get; set; } = new List<Question>();
        public List<Answer> SelectedAnswers { get; set; } = new List<Answer>();
        public ResultType ResultType { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime DateTime { get; set; }
        public User Reporter { get; set; }

        public int GetNumberGoodAnswers()
        {
            int goodAnswers = 0;
            foreach(Question q in QuestionsAsked)
            {
                if (q.IsQuestionCorrect(SelectedAnswers))
                {
                    goodAnswers++;
                }
            }
            return goodAnswers;
        }
    }

    public enum ResultType
    {
        Training, Exam
    }
}