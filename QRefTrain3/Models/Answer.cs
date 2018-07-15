using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public String Answertext { get; set; }
        public Boolean IsTrue { get; set; }
        public Boolean IsSelected { get; set; }


        public static Boolean IsAnswerCorrect(Answer answer, List<int> answerIds)
        {
            if (answerIds.Contains(answer.Id) && !answer.IsTrue || !answerIds.Contains(answer.Id) && answer.IsTrue)
            {
                return false;
            }
            return true;
        }
    }
}