using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.CustomException
{
    public class ValidationExamException : Exception
    {
        public ValidationExamException() { }

        public ValidationExamException(string message): base(message) { }

        public ValidationExamException(string message, Exception inner) : base(message, inner) { }
    }
}