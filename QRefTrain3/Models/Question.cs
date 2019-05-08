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
        public QuestionSubject Subject { get; set; }
        public string GifName { get; set; }
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public virtual List<Answer> Answers { get; set; }
        [Required]
        public string AnswerExplanation { get; set; }
        [Required]
        public string NationalGoverningBodies { get; set; }
        public virtual List<Exam> Exams { get; set; }
        public virtual List<Result> Results { get; set; }
        public virtual List<QuestionSuite> QuestionSuites { get; set; }

        public Question( QuestionSubject subject, string gifName, string questionText,
            List<Answer> answers, string answerExplanation, params NationalGoverningBody[] bodies)
        {
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

        public Question(QuestionSubject subject, string gifName, string questionText,
            List<Answer> answers, string answerExplanation, params string[] bodies)
        {
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
    }
}