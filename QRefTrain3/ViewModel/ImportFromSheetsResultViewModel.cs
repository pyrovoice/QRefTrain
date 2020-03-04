using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class ImportFromSheetsResultViewModel
    {
        public List<Question> untouchedQuestions = new List<Question>();
        public List<Question> retiredQuestions = new List<Question>();
        public List<Question> newlyCreatedQuestions = new List<Question>();
    }
}