using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRefTrain3.Controllers
{
    public class AdminController : BaseController
    {

        public ActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Homepage", "Home");
            }
            User user = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            if(user.UserRole != UserRole.Admin)
            {
                return RedirectToAction("Homepage", "Home");
            } 
            return View();
        }

        [HttpPost]
        public ActionResult AddQuestion(string QuestionTitle, string QuestionText, string Answer1, string Answer2, string Answer3, string Answer4, string Answer5, string Answer6, bool isAnswerTrue1, bool isAnswerTrue2, bool isAnswerTrue3, bool isAnswerTrue4, bool isAnswerTrue5, bool isAnswerTrue6, string Video, string Explanation, string QuestionSubject, List<string> NGBs)
        {
            List<Answer> answers = new List<Answer>();
            if(Answer1 != null && !Answer1.Equals("")) { answers.Add(new Answer(Answer1, isAnswerTrue1)); }
            if (Answer2 != null && !Answer2.Equals("")) { answers.Add(new Answer(Answer2, isAnswerTrue2)); }
            if (Answer3 != null && !Answer3.Equals("")) { answers.Add(new Answer(Answer3, isAnswerTrue3)); }
            if (Answer4 != null && !Answer4.Equals("")) { answers.Add(new Answer(Answer4, isAnswerTrue4)); }
            if (Answer5 != null && !Answer5.Equals("")) { answers.Add(new Answer(Answer5, isAnswerTrue5)); }
            if (Answer6 != null && !Answer6.Equals("")) { answers.Add(new Answer(Answer6, isAnswerTrue6)); }

            Question questionToAdd = new Question(QuestionTitle, (Models.QuestionSubject)Enum.Parse(typeof(Models.QuestionSubject), QuestionSubject), Video, QuestionText, answers, Explanation, NGBs.ToArray());
            Dal.Instance.CreateQuestion(questionToAdd);
            return RedirectToAction("Index");
        }
    }
}