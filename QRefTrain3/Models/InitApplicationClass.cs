using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class InitApplicationClass
    {
        public void InitApplication()
        {
            IDal dal = Dal.Instance;
            dal.reset();
            dal.CreateQuestion(ModelFactory.getDefaultQuestion());
            dal.CreateQuestion(ModelFactory.getDefaultQuestion());
            dal.CreateQuestion(ModelFactory.getDefaultQuestion());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionChaserBasic());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionChaserAdvanced());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionBeaterBasic());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionBeaterAdvanced());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionSeekerBasic());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionSeekerAdvanced());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionContactBasic());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionContactAdvanced());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionOtherBasic());
            dal.CreateQuestion(ModelFactory.getDefaultQuestionOtherAdvanced());
            dal.CreateUser(ModelFactory.getDefaultUser());
            dal.CreateUser(ModelFactory.getDefaultUser());

        }
    }
}