using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace QRefTrain3.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int TimeLimit { get; set; }
        public User User { get; set; }
        public List<Question> Questions { get; set; }
        public QuestionSuite Suite { get; set; }
    }
}