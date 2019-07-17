using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class QuestionSuite
    {
        public int Id { get; set; }
        public List<Question> Questions { get; set; }
        public User Owner { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int TimeLimit { get; set; }
    }
}