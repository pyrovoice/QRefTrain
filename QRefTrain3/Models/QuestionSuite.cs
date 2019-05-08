using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class QuestionSuite
    {
        public int Id { get; set; }
        public List<Question> questions { get; set; }
        public User owner { get; set; }
        public string code { get; set; }
        public string name { get; set; }
    }
}