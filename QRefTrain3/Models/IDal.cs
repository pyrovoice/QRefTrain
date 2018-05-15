using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public interface IDal : IDisposable
    {
        List<User> getAllUsers();
        List<Question> getAllQuestions();
        void CreateUser(User user);
        Question CreateQuestion(Question question);
        void reset();
        void DeleteUser(User user);
        void DeleteQuestion(Question question);

    }
}