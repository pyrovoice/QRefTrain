using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class QuizzViewModel
    {
        public List<Question> DisplayedQuestions { get; set; }
        public ResultType ResultType { get; set; }
    }
}