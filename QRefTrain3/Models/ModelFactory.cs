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
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Player should touch the round part of the hoop.", IsTrue = false }, new Answer() { Answertext = "Player should remove their broom before touching the hoop.", IsTrue = true,  }};
            if(type == AnswerType.MultipleAnswer)
            {
                answers.Add(new Answer() { Answertext = "Player should grab the hoop instead of touching it.", IsTrue = false });
                answers.Add(new Answer() { Answertext = "Nothing is missing.", IsTrue = false });
            }
            String questionName = field + " " + difficulty + " " + "All";
            questionCount++;
            return new Question(questionName, field, difficulty, true, "https://www.youtube.com/embed/BKSoi96X6fA?start=67&end=70", "What is missing from this procedure ?", type, answers, "The player should have the broom removed when touching the hoop.");
        }
    }
}
