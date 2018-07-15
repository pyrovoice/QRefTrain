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
            List<Result> results = Dal.Instance.GetNLastResultByUser(user, 10);
            List<ResultViewModel> viewModels = new List<ResultViewModel>();
            foreach(Result result in results)
            {
                viewModels.Add(new ResultViewModel(result));
            }
            return View("Profile", viewModels);
        }
        public ActionResult DisplayResultDetails(int id)
        {
            return View("ResultDetail", new ResultViewModel(Dal.Instance.GetResultById(id)));
        }

    }
}