using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class QuestionQuizzViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsVideo { get; set; }
        public string VideoURL { get; set; }
        public string QuestionText { get; set; }
        public AnswerType AnswerType { get; set; }
        public List<AnswerQuizzViewModel> Answers { get; set; }
        
        // Lists containing user's answers. They have to be separated because RadioButtonFor and CheckBoxFor are incompatible when used together

        // Use this to store checkbox answers : Boolean switch. Key is answer's ID, value is True/false (selected or not)
        public Dictionary<int, Boolean> AnswerCheckbox { get; set; }
        // Use this to store radioButton answers : All radioButtons register to the same list, thus being in the same group
        // Key is Question's ID, value is selected Answers' Id
        public Dictionary<int, List<int>> AnswersRadio { get; set; }
    }
}