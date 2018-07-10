using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class AnswerQuizzViewModel
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public bool IsSelected { get; set; }
    }
}