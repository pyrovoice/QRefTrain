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
            return View();
        }

        [HttpPost]
        public ActionResult CreateQuestionSuite()
        {
            return View("CreateQuestionSuite");
        }
    }
}