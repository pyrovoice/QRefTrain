using QRefTrain3.Models;
using QRefTrain3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QRefTrain3.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            UserViewModel viewModel = new UserViewModel { IsAutenthified = HttpContext.User.Identity.IsAuthenticated };
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.User = Dal.Instance.GetUserByName(HttpContext.User.Identity.Name);
            }
            return View("Login", viewModel);
        }
        [HttpPost]
        public ActionResult Index(UserViewModel viewModel, string returnUrl)
        {
            User user = Dal.Instance.Authenticate(viewModel.User.Name, viewModel.User.Password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Name, false);
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return Redirect("/");
            }
            ModelState.AddModelError("User.Name", "Name or Password incorrect");

            return View("Login", viewModel);
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isAllValid = true;
                if (Dal.Instance.UsernameAlreadyInDB(registerViewModel.Name))
                {
                    ViewBag.UsernameAlreadyInDB = "Username is already used";
                    isAllValid = false;
                }
                if (Dal.Instance.MailAlreadyInDB(registerViewModel.Email))
                {
                    ViewBag.MailError = "Mail is already used";
                    isAllValid = false;
                }

                User newUser = new User() { Name = registerViewModel.Name, Email = registerViewModel.Email, Password = registerViewModel.Password };

                if (!SendMail(newUser)){
                    ViewBag.MailError = "Cannot send a mail. Please verify the adress, or send a request (it could be us)";
                    isAllValid = false;
                }
                if (isAllValid)
                {
                    Dal.Instance.CreateUser(newUser);
                    newUser = Dal.Instance.GetUserByName(newUser.Name);

                    return View("CreateAccountConfirmation");
                }
            }
            return View("CreateAccount", registerViewModel);
        }

        

        public ActionResult Disconnect()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        private bool SendMail(User user)
        {
            SmtpClient SmtpServer = new SmtpClient("smtp.live.com");
            var mail = new MailMessage();
            mail.From = new MailAddress("maxime.grazzini@hotmail.com");
            mail.To.Add(user.Email);
            mail.Subject = "Your Sub";
            mail.IsBodyHtml = true;
            mail.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", user.Name, Url.Action("ConfirmEmail", "Login", new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential("maxime.grazzini@hotmail.com", "QzsaErty");
            SmtpServer.EnableSsl = true;
            try
            {
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;

        }
    }
}