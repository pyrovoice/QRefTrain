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
        public QuizType ResultType { get; set; }
        public DateTime? StartTime { get; set; }
        public int? TimeLimit { get; set; }
        public int? QuestionSuiteId { get; set; }

        public QuizViewModel()
        {

        }

        public QuizViewModel(QuizType type, DateTime? startTime, QuizTemplate suite) : this(type, startTime, suite.TimeLimit, suite.Questions, suite.Id)
        {
        }

        public QuizViewModel(Quiz exam) : this(QuizType.Exam, exam.StartDate, exam.Suite.TimeLimit, exam.Suite.Questions, exam.Suite.Id)
        {
        }

        public QuizViewModel(QuizType type, DateTime? startTime, int eXAM_TIME_LIMIT, List<Question> questions): this(type, startTime, eXAM_TIME_LIMIT, questions, null)
        {
        }

        public QuizViewModel(QuizType resultType, List<Question> list): this(resultType, null, null, list, null)
        {
        }

        public QuizViewModel(QuizType type, DateTime? startTime, int? timeLimit, List<Question> questions, int? questionSuiteId)
        {
            this.ResultType = type;
            this.StartTime = startTime;
            this.TimeLimit = timeLimit;
            foreach (Question question in questions)
            {
                DisplayedQuestions.Add(new QuestionQuizzViewModel(question));
            }
            this.QuestionSuiteId = questionSuiteId;
        }

    }
}