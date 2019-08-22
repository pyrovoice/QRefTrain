using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace QRefTrain3.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        //Who passes the test
        public virtual User User { get; set; }
        //What QuestionSuite should this exam be based on
        public virtual QuestionSuite Suite { get; set; }
    }
}