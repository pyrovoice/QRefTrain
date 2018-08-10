using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime LogTime { get; set; }
        public int? UserId { get; set; }
        public String LogText { get; set; }
    }
}