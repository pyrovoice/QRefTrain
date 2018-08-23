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
                RedirectToAction("Homepage", "Home");
            }
            User user = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            if(user.UserRole != UserRole.Admnin)
            {
                RedirectToAction("Homepage", "Home");
            } 
            return View();
        }
    }
}