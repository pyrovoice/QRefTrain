using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public enum QuestionSubject
    {
        Contacts, Keeper, FieldsAndEquipment, OutOfBoundaries, Process, DelayOfGame, Definition, Advantage, DelayedPenalty, Scoring, SnitchPlay, ImmunityAndGuarding, Reset, Substitution, NHNF, KO, Other, BallInteraction
    };
    public enum NationalGoverningBody
    {
        All, Europe, Usa, Canada
    };

    public class Question
    {
        public int Id { get; set; }
        [Required]
        public string PublicId { get; set; }
        [Required]
        public QuestionSubject Subject { get; set; }
        public string GifName { get; set; } = null;
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public virtual List<Answer> Answers { get; set; }
        public string AnswerExplanation { get; set; }
        [Required]
        public string NationalGoverningBodies { get; set; }
        //Indicate whether a question has been replaced or removed, and thus should not be used anymore.
        public bool IsRetired { get; set; } = false;
        public virtual List<Result> Results { get; set; }
        public virtual List<QuestionSuite> QuestionSuites { get; set; }

        public Question(string publicId, QuestionSubject subject, string gifName, string questionText,
            List<Answer> answers, string answerExplanation, params NationalGoverningBody[] bodies)
        {
            this.PublicId = publicId;
            this.Subject = subject;
            this.GifName = String.IsNullOrEmpty(gifName) ? null : gifName;
            this.QuestionText = questionText;
            this.Answers = answers;
            this.AnswerExplanation = answerExplanation;
            if (bodies.Contains<NationalGoverningBody>(Models.NationalGoverningBody.All) || bodies.Count() == 0 || bodies == null)
            {
                this.NationalGoverningBodies = "ALL";
            }
            else
            {
                this.NationalGoverningBodies = string.Join(";", bodies);
            }
        }

        public Question(string publicId, QuestionSubject subject, string gifName, string questionText,
            List<Answer> answers, string answerExplanation, params string[] bodies)
        {
            this.PublicId = publicId;
            this.Subject = subject;
            this.GifName = String.IsNullOrEmpty(gifName) ? null : gifName;
            this.QuestionText = questionText;
            this.Answers = answers;
            this.AnswerExplanation = answerExplanation;
            if (bodies == null || bodies.Contains(Models.NationalGoverningBody.All.ToString()) || bodies.Count() == 0)
            {
                this.NationalGoverningBodies = "ALL";
            }
            else
            {
                this.NationalGoverningBodies = string.Join(";", bodies);
            }
        }

        public Question()
        {

        }

        public static Boolean IsQuestionCorrect(Question question, List<int> answerIds)
        {
            foreach (Answer a in question.Answers)
            {
                if ((answerIds.Contains(a.Id) && !a.IsTrue) || (!answerIds.Contains(a.Id) && a.IsTrue))
                {
                    return false;
                }
            }
            return true;
        }

        public Boolean IsQuestionCorrect(List<Answer> answer)
        {
            foreach (Answer a in this.Answers)
            {
                if ((answer.Contains(a) && !a.IsTrue) || (!answer.Contains(a) && a.IsTrue))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Compare all fields of two questions, except ID, to determine whether they contain the same data or not.
        /// Usefull when importing Questions.
        /// </summary>
        /// <param name="otherQ"></param>
        /// <returns></returns>
        public Boolean IsSimilar(Question otherQ)
        {
            if (!this.PublicId.Equals(otherQ.PublicId))
            {
                return false;
            }
            if (this.GifName == null && otherQ.GifName != null)
            {
                return false;
            }
            if (this.GifName != null && otherQ.GifName == null)
            {
                return false;
            }

            if (this.GifName != null && !this.GifName.Equals(otherQ.GifName))
            {
                return false;
            }

            if (this.Subject != otherQ.Subject)
            {
                return false;
            }
            if (!this.QuestionText.Equals(otherQ.QuestionText))
            {
                return false;
            }
            if (!this.AnswerExplanation.Equals(otherQ.AnswerExplanation))
            {
                return false;
            }
            if (this.NationalGoverningBodies != otherQ.NationalGoverningBodies)
            {
                return false;
            }
            if (this.Answers.Count != otherQ.Answers.Count)
            {
                return false;
            }
            foreach (Answer a in this.Answers)
            {
                Boolean isFound = false;
                foreach (Answer otherA in otherQ.Answers)
                {
                    if (otherA.IsSimilar(a))
                    {
                        isFound = true;
                        break;
                    }
                }
                if (!isFound)
                {
                    return false;
                }
            }
            return true;
        }
    }
}