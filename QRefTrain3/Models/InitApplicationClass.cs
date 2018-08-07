using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class InitApplicationClass
    {
        /**
         * Create test data, one question of each type and difficulty, one question Other Basic multiple answers possible, and one user1 pw : password
         */
        public void InitApplication()
        {
            IDal dal = Dal.Instance;
            dal.reset();
            // Create one of each question
            foreach (NationalGoverningBody body in Enum.GetValues(typeof(NationalGoverningBody)))
            {
                foreach (QuestionField field in Enum.GetValues(typeof(QuestionField))) {
                    foreach (QuestionDifficulty difficulty in Enum.GetValues(typeof(QuestionDifficulty))) {
                        dal.CreateQuestion(ModelFactory.GetQuestion(field, difficulty, AnswerType.MultipleAnswer, body));
                    }
                }
            }
            // Add a long list of question to test pages with lots of questions
            for (int i = 0; i < 8; i++)
            {
                dal.CreateQuestion(ModelFactory.GetQuestion(QuestionField.Other, QuestionDifficulty.Advanced, AnswerType.MultipleAnswer));
            }
            // Add a question with multiple NGBs but not ALL
            dal.CreateQuestion(ModelFactory.GetQuestion(QuestionField.Other, QuestionDifficulty.Basic, AnswerType.MultipleAnswer, NationalGoverningBody.Canada, NationalGoverningBody.Usa));
            dal.CreateQuestion(ModelFactory.GetQuestion(QuestionField.Other, QuestionDifficulty.Basic, AnswerType.MultipleAnswer, NationalGoverningBody.Canada, NationalGoverningBody.Europe));
            dal.CreateUser(ModelFactory.GetDefaultUser());

        }
    }
}