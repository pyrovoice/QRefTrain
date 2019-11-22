using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace QRefTrain3.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public User User { get; set; }
        public QuizTemplate Suite { get; set; }
        public Boolean IsClosed { get; set; } = false;

        public Quiz()
        {
        }

        public Quiz(DateTime startDate, User user, QuizTemplate suite)
        {
            StartDate = startDate;
            User = user;
            Suite = suite;
        }
    }
}