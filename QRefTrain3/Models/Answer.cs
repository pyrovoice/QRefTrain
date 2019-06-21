using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class Answer : IComparable
    {
        public int Id { get; set; }
        public string Answertext { get; set; }
        public bool IsTrue { get; set; }
        public virtual List<Result> Results { get; set; } = new List<Result>();

        private static readonly string[] commonAnswers = { "NoPenalty", "backToHoops", "Turnover", "BlueCard", "YellowCard", "RedCard" };

        public Answer() { }

        public static Boolean IsAnswerCorrect(Answer answer, List<int> answerIds)
        {
            if (answerIds.Contains(answer.Id) && !answer.IsTrue || !answerIds.Contains(answer.Id) && answer.IsTrue)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sort answer by text, using the folliwing rule :
        /// <para />An answer having a text that's a common text answer takes priority over a text that's not in the common answers.
        /// <para />If two answers have a common text, the order in the array is used (from "NoPenalty" to "RedCard")
        /// <para />If no answer has a common text, use the classic string comparison
        /// </summary>
        /// <param name="obj">The other Answer. No two answers should have the same text.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Answer otherAnswer = ((Answer)obj);
            if (commonAnswers.Contains(this.Answertext) && commonAnswers.Contains(otherAnswer.Answertext))
            {
                return Array.IndexOf(commonAnswers, this.Answertext) < Array.IndexOf(commonAnswers, otherAnswer.Answertext) ? -1 : 1;
            }
            else if (commonAnswers.Contains(this.Answertext) || commonAnswers.Contains(otherAnswer.Answertext))
            {
                return commonAnswers.Contains(this.Answertext) ? -1 : 1;
            }
            else
            {
                return this.Answertext.CompareTo(otherAnswer.Answertext);
            }
        }

        public Answer(string answerText, bool isTrue)
        {
            this.Answertext = answerText;
            this.IsTrue = isTrue;
        }

        public bool IsSimilar(Answer otherA)
        {
            if(this.IsTrue != otherA.IsTrue)
            {
                return false;
            }
            if (!this.Answertext.Equals(otherA.Answertext))
            {
                return false;
            }
            return true;
        }
    }
}