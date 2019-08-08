using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class QuizzViewModel
    {
        public List<QuestionQuizzViewModel> DisplayedQuestions { get; set; } = new List<QuestionQuizzViewModel>();
        public ResultType ResultType { get; set; }
        public DateTime? StartTime { get; set; }
        public int? TimeLimit { get; set; }
        public QuestionSuite Suite { get; set; }

        public QuizzViewModel()
        {

        }

        public QuizzViewModel(List<Question> displayedQuestions, ResultType type, DateTime? startTime, int? timeLimit, QuestionSuite suite)
        {
            foreach (Question question in displayedQuestions)
            {
                DisplayedQuestions.Add(new QuestionQuizzViewModel(question));
            }
            this.ResultType = type;
            this.StartTime = startTime;
            this.Suite = suite;
            this.TimeLimit = timeLimit;
        }

        public QuizzViewModel(Exam exam)
        {
            foreach (Question question in exam.Questions)
            {
                DisplayedQuestions.Add(new QuestionQuizzViewModel(question));

            }
            this.ResultType = ResultType.Exam;
            this.StartTime = exam.StartDate;
            this.Suite = exam.Suite;
            this.TimeLimit = exam.TimeLimit;
        }
    }
}