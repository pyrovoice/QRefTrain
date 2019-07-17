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
        public int? SuiteID { get; set; }

        public QuizzViewModel()
        {

        }

        public QuizzViewModel(List<Question> displayedQuestions, ResultType type, DateTime? startTime, int? timeLimit, int? suiteID)
        {
            foreach (Question question in displayedQuestions)
            {
                DisplayedQuestions.Add(new QuestionQuizzViewModel(question));
            }
            this.ResultType = type;
            this.StartTime = startTime;
            this.SuiteID = suiteID;
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
            this.SuiteID = exam.SuiteId;
            this.TimeLimit = exam.TimeLimit;
        }
    }
}