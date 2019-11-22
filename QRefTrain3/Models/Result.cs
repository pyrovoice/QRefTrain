using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{

    public enum QuizType
    {
        Training, Exam
    }

    public class Result
    {

        public int Id { get; set; }
        public User User { get; set; }
        public virtual List<Question> QuestionsAsked { get; set; }
        public virtual List<Answer> SelectedAnswers { get; set; } 
        public QuizType QuizType { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime DateTime { get; set; }
        public QuizTemplate QuestionSuite { get; set; }

        public Result()
        {
        }

        public Result(User userTakingTheTest, List<Question> questionsAsked, List<Answer> selectedAnswers, QuizType quizType, DateTime dateTime, QuizTemplate questionSuite)
        {
            User = userTakingTheTest;
            QuestionsAsked = questionsAsked;
            SelectedAnswers = selectedAnswers;
            QuizType = quizType;
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