﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class QuizTemplate
    {
        public int Id { get; set; }
        public virtual List<Question> Questions { get; set; }
        // If null, the template is temporary
        public virtual User Owner { get; set; }
        public string Code { get; set; }
        public string Name { get; set; } 
        public int TimeLimit { get; set; }

        public QuizTemplate() { }

        public QuizTemplate(List<Question> questions, User owner, string name, int timeLimit)
        {
            Questions = questions;
            Owner = owner;
            Code = GenerateNewCode();
            Name = name;
            TimeLimit = timeLimit;
        }

        /// <summary>
        /// Generate a new 6 character code at random using all upper case letters and numbers, then checks that the code don't already exist.
        /// </summary>
        /// <returns>The new code generated</returns>
        private String GenerateNewCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code;
            do
            {
                code = new string(Enumerable.Repeat(chars, 6)
                  .Select(s => s[random.Next(s.Length)]).ToArray());

            } while (Dal.Instance.GetQuestionSuiteByCode(code) != null);
            return code;

        }
    }
}