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
            foreach(QuestionField field in Enum.GetValues(typeof(QuestionField))){
                foreach (QuestionDifficulty difficulty in Enum.GetValues(typeof(QuestionDifficulty))){
                    dal.CreateQuestion(ModelFactory.GetQuestion(field, difficulty, AnswerType.MultipleAnswer));
                }
            }
            for(int i = 0; i <8; i++)
            {
                dal.CreateQuestion(ModelFactory.GetQuestion(QuestionField.Other, QuestionDifficulty.Advanced, AnswerType.MultipleAnswer));
            }
            dal.CreateUser(ModelFactory.GetDefaultUser());

        }
    }
}