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
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult DisplayProfile(string name)
        {
            User user = null;
            if (String.IsNullOrEmpty(name))
            {
                user = Dal.Instance.GetUserByName(name);
            }
            else if (User.Identity.IsAuthenticated)
            {
                user = Dal.Instance.GetUserByName(User.Identity.Name);
            }
            else
            {
                RedirectToAction("Homepage", "Home");
            }
            List<Result> results = Dal.Instance.GetNLastResultByUser(user, 10);
            List<ResultViewModel> viewModels = new List<ResultViewModel>();
            foreach (Result result in results)
            {
                viewModels.Add(new ResultViewModel(result));
            }
            int userRank = UserInfoHelper.GetUserRank(user);
            return View("Profile", new ProfileViewModel(viewModels, userRank));
        }

        public ActionResult DisplayResultDetails(int id)
        {
            return View("ResultDetail", new ResultViewModel(Dal.Instance.GetResultById(id)));
        }

    }
}