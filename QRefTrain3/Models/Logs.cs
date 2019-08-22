using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class Log
    {
        public int Id { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime LogTime { get; set; }
        public virtual User User { get; set; }
        public String LogText { get; set; }
    }
}