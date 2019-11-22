using QRefTrain3.Helper;
using QRefTrain3.Models;
using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QRefTrain3.Controllers
{
    public class HomeController : BaseController
    {
        //2mn30
        private static readonly int EXAM_TIME_LIMIT = 1500000;
        private static readonly int EXAM_NBR_QUESTIONS = 10;

        public ActionResult Homepage()
        {
            ViewBag.ErrorQuiz = TempData["ErrorQuiz"];
            Quiz exam = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                exam = Dal.Instance.GetOngoingExamByUsername(HttpContext.User.Identity.Name);
            }
            return View(exam != null);
        }

        

        [HttpPost]
        public ActionResult UpdateLanguage(String userSelectedLanguage)
        {
            userSelectedLanguage = CultureHelper.GetImplementedOrDefaultCulture(userSelectedLanguage);
            CookieHelper.UpdateCookie(Request, Response, CookieNames.languageCookie, userSelectedLanguage, DateTime.Now.AddYears(1));
            /*
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = languages;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture")
                {
                    Value = languages,
                    Expires = DateTime.Now.AddYears(1)
                };
            }
            Response.Cookies.Add(cookie);
            */
            return RedirectToAction("Homepage");
        }
    }
}