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

    public enum LogType
    {
        MAIL_SENT, QUIZ_FINISHED, QUESTION_REPORTED, ERROR
    }

    public class Log : IComparable<Log>
    {
        public int Id { get; set; }
        public LogLevel Level { get; set; }
        public String LogText { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime LogTime { get; set; }
        public LogType LogType { get; set; }
        //Virtual = nullable for entity framwork
        public virtual User User { get; set; }

        public Log(LogLevel level, LogType logType, string logText, DateTime logTime, User user)
        {
            Level = level;
            LogText = logText;
            LogTime = logTime;
            User = user;
            LogType = logType;
        }

        public Log() { }

        public int CompareTo(Log other)
        {
            if(other.LogTime == this.LogTime)
            {
                return 0;
            }
            return this.LogTime > other.LogTime ? -1 : 1;
        }
    }
}