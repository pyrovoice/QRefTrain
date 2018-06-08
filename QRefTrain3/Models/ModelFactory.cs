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

        public static User GetDefaultUser()
        {
            String userName = "user" + userCount;
            userCount++;
            return new User() { Name = userName, Email = userName + "@mail.com", Password = "password" };
        }

        public static Question GetQuestion(QuestionField field, QuestionDifficulty difficulty, AnswerType type)
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "1rst good answer", IsTrue = true }, new Answer() { Answertext = "1rst wrong answer", IsTrue = false,  }};
            if(type == AnswerType.MultipleAnswer)
            {
                answers.Add(new Answer() { Answertext = "2sd good answer", IsTrue = true });
                answers.Add(new Answer() { Answertext = "2sd wrong answer", IsTrue = false });
            }
            String questionName = "Question" + questionCount;
            questionCount++;
            return new Question(questionName, field, difficulty, true, "https://www.youtube.com/embed/fp3rkJTzI_Q?autoplay=0", "Est-ce une vidéo de Cyprien ?", type, answers, "Answer explanation. Yes, that's a placeholder text. Wadduyou gonna do about it ?");

        }
    }
}
