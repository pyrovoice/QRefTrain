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
        Contacts, Keeper, FieldsAndEquipment, OutOfBoundaries, Process, DelayOfGame, Definition, Advantage, DelayedPenalty, Scoring, SnitchPlay, ImmunityAndGuarding, Reset, Substitution, NHNF, KO, Other
    };
    public enum NationalGoverningBody
    {
        All, Europe, Usa, Canada
    };

    public class Question
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Question name not valid")]
        public string Name { get; set; }
        [Required]
        public QuestionSubject Subject { get; set; }
        [Required]
        public bool IsVideo { get; set; }
        public string VideoURL { get; set; }
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public List<Answer> Answers { get; set; }
        [Required]
        public string AnswerExplanation { get; set; }
        [Required]
        public string NationalGoverningBodies { get; set; }
        public List<Exam> Exams { get; set; }

        public Question(string name, QuestionSubject subject, bool isVideo, string videoUrl, string questionText, 
            List<Answer> answers, string answerExplanation, params NationalGoverningBody[] bodies)
        {
            this.Name = name;
            this.Subject = subject;
            this.IsVideo = isVideo;
            if (IsVideo == true) { this.VideoURL = videoUrl; } else { this.VideoURL = null; }
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

        public Question()
        {

        }

        public static Boolean IsQuestionCorrect(Question question, List<int> answerIds)
        {
            foreach(Answer a in question.Answers)
            {
                if((answerIds.Contains(a.Id) && !a.IsTrue) || (!answerIds.Contains(a.Id) && a.IsTrue))
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