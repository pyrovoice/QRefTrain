using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public enum QuestionField
    {
        Chaser, Beater, Seeker, Contact, Process, Other
    };
    public enum QuestionDifficulty
    {
        Basic, Advanced
    };
    public enum AnswerType
    {
        //SingleAnswer,
        MultipleAnswer
    }

    public class Question
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Question name not valid")]
        public string Name { get; set; }
        [Required]
        public QuestionField Field { get; set; }
        [Required]
        public QuestionDifficulty Difficulty { get; set; }
        [Required]
        public bool IsVideo { get; set; }
        public string VideoURL { get; set; }
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public AnswerType AnswerType { get; set; }
        [Required]
        public List<Answer> Answers { get; set; }
        [Required]
        public String AnswerExplanation { get; set; }

        public Question(string name, QuestionField field, QuestionDifficulty difficulty, bool isVideo, string videoUrl, string questionText, AnswerType answerType,
            List<Answer> answers, string answerExplanation)
        {
            this.Name = name;
            this.Field = field;
            this.Difficulty = difficulty;
            this.IsVideo = isVideo;
            if (IsVideo == true) { this.VideoURL = videoUrl; } else { this.VideoURL = null; }
            this.QuestionText = questionText;
            this.AnswerType = answerType;
            this.Answers = answers;
            this.AnswerExplanation = answerExplanation;
        }
        public Question()
        {

        }
        public void SetQuestionField(String newQuestionField)
        {
            QuestionField qField = (QuestionField)Enum.Parse(typeof(QuestionField), newQuestionField);
            if(Enum.IsDefined(typeof(QuestionField), qField))
            {
                this.Field = qField;
            }
        }

        public static Boolean IsGoodAnswer(Question q)
        {
            foreach (Answer a in q.Answers)
            {
                // If wrong answer is selected or good answer is not, then return false
                if ((a.IsSelected && !a.IsTrue) || (!a.IsSelected && a.IsTrue))
                {
                    return false;
                }
            }
            return true;
        }
    }
}