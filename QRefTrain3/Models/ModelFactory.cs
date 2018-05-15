using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class ModelFactory
    {
        public static int userCount = 0;
        public static int questionCount = 0;
        public static User getDefaultUser()
        {
            String userName = "user" + userCount;
            userCount++;
            return new User() { Name = userName, Email = userName + "@mail.com", Password = "password" };
        }

        public static Question getDefaultQuestion()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "oui", IsTrue = true }, new Answer() { Answertext = "non", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            questionCount++;
            return new Question(questionName, QuestionField.Other, QuestionDifficulty.Basic, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", "Est-ce une vidéo de Cyprien ?", AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");

        }

        public static Question getDefaultQuestionChaserBasic()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            questionCount++;
            return new Question(questionName, QuestionField.Chaser, QuestionDifficulty.Basic, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", "Chaser Basic", AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionChaserAdvanced()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            questionCount++;
            return new Question(questionName, QuestionField.Chaser, QuestionDifficulty.Advanced, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", "Chaser Advanced", AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionBeaterBasic()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            QuestionField qField = QuestionField.Beater;
            QuestionDifficulty qDifficulty = QuestionDifficulty.Basic;
            questionCount++;
            return new Question(questionName, qField, qDifficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", qField.ToString() + " " + qDifficulty.ToString(), AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionBeaterAdvanced()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            QuestionField qField = QuestionField.Beater;
            QuestionDifficulty qDifficulty = QuestionDifficulty.Advanced;
            questionCount++;
            return new Question(questionName, qField, qDifficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", qField.ToString() + " " + qDifficulty.ToString(), AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionContactBasic()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            QuestionField qField = QuestionField.Contact;
            QuestionDifficulty qDifficulty = QuestionDifficulty.Basic;
            questionCount++;
            return new Question(questionName, qField, qDifficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", qField.ToString() + " " + qDifficulty.ToString(), AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionContactAdvanced()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            QuestionField qField = QuestionField.Contact;
            QuestionDifficulty qDifficulty = QuestionDifficulty.Advanced;
            questionCount++;
            return new Question(questionName, qField, qDifficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", qField.ToString() + " " + qDifficulty.ToString(), AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionSeekerBasic()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            QuestionField qField = QuestionField.Seeker;
            QuestionDifficulty qDifficulty = QuestionDifficulty.Basic;
            questionCount++;
            return new Question(questionName, qField, qDifficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", qField.ToString() + " " + qDifficulty.ToString(), AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionSeekerAdvanced()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true,  }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            QuestionField qField = QuestionField.Seeker;
            QuestionDifficulty qDifficulty = QuestionDifficulty.Advanced;
            questionCount++;
            return new Question(questionName, qField, qDifficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", qField.ToString() + " " + qDifficulty.ToString(), AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionOtherBasic()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true,  }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            QuestionField qField = QuestionField.Other;
            QuestionDifficulty qDifficulty = QuestionDifficulty.Basic;
            questionCount++;
            return new Question(questionName, qField, qDifficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", qField.ToString() + " " + qDifficulty.ToString(), AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }

        public static Question getDefaultQuestionOtherAdvanced()
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Bonne réponse", IsTrue = true,  }, new Answer() { Answertext = "Mauvaise réponse", IsTrue = false,  }, };
            String questionName = "Question" + questionCount;
            QuestionField qField = QuestionField.Other;
            QuestionDifficulty qDifficulty = QuestionDifficulty.Advanced;
            questionCount++;
            return new Question(questionName, qField, qDifficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", qField.ToString() + " " + qDifficulty.ToString(), AnswerType.SingleAnswer, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");
        }
    }
}
