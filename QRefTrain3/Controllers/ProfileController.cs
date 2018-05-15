using QRefTrain3.Models;
using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRefTrain3.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult DisplayProfile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                RedirectToAction("Homepage", "Home");
            }
            User user = Dal.Instance.GetUserByName(User.Identity.Name);
            List<Result> results = Dal.Instance.Get10ResultByUser(user);
            return View("Profile", results);
        }
        public ActionResult DisplayResultDetails(int id)
        {
            List<QuestionViewModel> questions = new List<QuestionViewModel>();
            Result result = Dal.Instance.GetResultById(id);
            foreach (int questionId in result.QuestionsAskedIds)
            {
                questions.Add(new QuestionViewModel { Question = Dal.Instance.GetQuestionById(questionId), AnsweredRight = (result.GoodAnswersIds.Contains(questionId) ? true : false) });
            }
            return View("ResultDetail", questions);
        }

    }
}