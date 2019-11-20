using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{

    public enum LogLevel
    {
        TRACE, INFO, WARN, ERROR, FATAL
    }

    public class Log
    {
        public int Id { get; set; }
        [Column(TypeName = "DateTime2")]
        public LogLevel Level { get; set; }
        public String LogText { get; set; }
        public DateTime LogTime { get; set; }
        public int? UserId { get; set; }


        public Log(LogLevel level, string message, DateTime dateTime, int id)
        {
            this.Level = level;
            this.LogText = message;
            this.LogTime = dateTime;
            this.UserId = id;
        }
    }
}