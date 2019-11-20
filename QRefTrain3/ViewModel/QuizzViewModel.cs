using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class QuizViewModel
    {
        public List<QuestionQuizzViewModel> DisplayedQuestions { get; set; } = new List<QuestionQuizzViewModel>();
        public ResultType ResultType { get; set; }
        public DateTime? StartTime { get; set; }
        public int? TimeLimit { get; set; }
        public QuestionSuite Suite { get; set; }

        public QuizViewModel()
        {

        }

        public QuizViewModel(ResultType type, DateTime? startTime, QuestionSuite suite)
        {
            foreach (Question question in suite.Questions)
            {
                DisplayedQuestions.Add(new QuestionQuizzViewModel(question));
            }
            this.ResultType = type;
            this.StartTime = startTime;
            this.Suite = suite;
        }

        public QuizViewModel(ResultType type, DateTime? startTime, int? timeLimit, List<Question> questions)
        {
            foreach (Question question in questions)
            {
                DisplayedQuestions.Add(new QuestionQuizzViewModel(question));
            }
            this.ResultType = type;
            this.StartTime = startTime;
            this.Suite = null;
            this.TimeLimit = timeLimit;
        }

        public QuizViewModel(Exam exam)
        {
            foreach (Question question in exam.Suite.Questions)
            {
                DisplayedQuestions.Add(new QuestionQuizzViewModel(question));

            }
            this.ResultType = ResultType.Exam;
            this.StartTime = exam.StartDate;
            this.Suite = exam.Suite;
            this.TimeLimit = exam.Suite.TimeLimit;
        }
    }
}