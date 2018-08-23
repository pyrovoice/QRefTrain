﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class ModelFactory
    {
        public static int userCount = 0;

        public static User GetDefaultUser()
        {
            String userName = "user" + userCount;
            userCount++;
            return new User() { Name = userName, Email = userName + "@mail.com", Password = "password" };
        }

        public static Question GetQuestion(QuestionSubject  subject, params NationalGoverningBody[] bodies)
        {
            List<Answer> answers = new List<Answer>() { new Answer() { Answertext = "Question_Sample_Answer1", IsTrue = false }, new Answer() { Answertext = "Question_Sample_Answer2", IsTrue = true,  }};
            answers.Add(new Answer() { Answertext = "Question_Sample_Answer3", IsTrue = false });
            answers.Add(new Answer() { Answertext = "Question_Sample_Answer4", IsTrue = false });
            return new Question("Question_Sample_Title", subject, true, "https://www.youtube.com/embed/BKSoi96X6fA?start=67&end=70", "Question_Sample_Text", answers, "Question_Sample_Explanation", bodies);
        }
    }
}
