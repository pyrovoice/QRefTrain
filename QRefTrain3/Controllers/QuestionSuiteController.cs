using QRefTrain3.Models;
using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRefTrain3.Controllers
{
    public class QuestionSuiteController : Controller
    {
        // GET: QuestionSuite
        public ActionResult Index()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Homepage", "Home");
            }
            User currentUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            QuestionSuiteViewModel qsvm = new QuestionSuiteViewModel()
            {
                PreviouslyCreatedQuestionSuiteCode = null,
                UserQuestionSuites = Dal.Instance.GetQuestionSuiteByUser(currentUser)
            };
            return View(qsvm);
        }

        [HttpPost]
        public ActionResult MoveToQuestionSuite()
        {
            return View("MoveToQuestionSuite");
        }

        [HttpPost]
        public ActionResult CreateQuestionSuite(String name,  List<string> questionIds)
        {
            //Create the suite
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Homepage", "Home");
            }
            User currentUser = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            QuestionSuite newQuestionSuite = new QuestionSuite()
            {
                code = GenerateNewCode(),
                name = name,
                owner = currentUser,
                questions = Dal.Instance.GetQuestionsById(questionIds)
            };

            //Update db with new suite
            Dal.Instance.CreateQuestionSuite(newQuestionSuite);

            QuestionSuiteViewModel qsvm = new QuestionSuiteViewModel()
            {
                PreviouslyCreatedQuestionSuiteCode = newQuestionSuite.code,
                UserQuestionSuites = Dal.Instance.GetQuestionSuiteByUser(currentUser)
            };
            //Return to the main page and add a field to copy the suite's ID
            return View("Index", qsvm);
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