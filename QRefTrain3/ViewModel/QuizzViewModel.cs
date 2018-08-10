using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class QuizzViewModel
    {
        public List<QuestionQuizzViewModel> DisplayedQuestions { get; set; }
        public ResultType ResultType { get; set; }
        public int? ResultId { get; set; }

        public QuizzViewModel(List<Question> displayedQuestions, ResultType type) : this(displayedQuestions, type, null)
        {
            
        }

        public QuizzViewModel()
        {

        }

        public QuizzViewModel(List<Question> displayedQuestions, ResultType type, int? id)
        {
            DisplayedQuestions = new List<QuestionQuizzViewModel>();
            foreach (Question question in displayedQuestions)
            {
                DisplayedQuestions.Add(new QuestionQuizzViewModel(question));

            }
            this.ResultType = type;
            this.ResultId = id;
        }
    }
}