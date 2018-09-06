using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Answertext { get; set; }
        public bool IsTrue { get; set; }
        public bool IsSelected { get; set; }
        public virtual List<Result> Results { get; set; } = new List<Result>();
        public virtual List<Question> Questions { get; set; } = new List<Question>();

        public Answer() { }

        public static Boolean IsAnswerCorrect(Answer answer, List<int> answerIds)
        {
            if (answerIds.Contains(answer.Id) && !answer.IsTrue || !answerIds.Contains(answer.Id) && answer.IsTrue)
            {
                return false;
            }
            return true;
        }

        public Answer(string answerText, bool isTrue)
        {
            this.Answertext = answerText;
            this.IsTrue = isTrue;
            this.IsSelected = false;
        }
    }
}