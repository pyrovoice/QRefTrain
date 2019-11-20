using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{

    public enum ResultType
    {
        Training, Exam
    }

    public class Result
    {

        public int Id { get; set; }
        public User User { get; set; }
        public List<Question> QuestionsAsked { get; set; } = new List<Question>();
        public List<Answer> SelectedAnswers { get; set; } = new List<Answer>();
        public ResultType ResultType { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime DateTime { get; set; }
        public QuestionSuite QuestionSuite { get; set; }

        public Result(User userTakingTheTest, List<Question> questionsAsked, List<Answer> selectedAnswers, ResultType resultType, DateTime dateTime, QuestionSuite questionSuite)
        {
            User = userTakingTheTest;
            QuestionsAsked = questionsAsked;
            SelectedAnswers = selectedAnswers;
            ResultType = resultType;
            DateTime = dateTime;
            QuestionSuite = questionSuite;
        }

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

}