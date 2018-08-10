using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class Result
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<int> QuestionsAskedIds { get; set; } = new List<int>();
        public List<int> SelectedAnswers { get; set; } = new List<int>();
        public ResultType ResultType { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime DateTime { get; set; }
        
    }

    public enum ResultType
    {
        Training, Exam
    }
}