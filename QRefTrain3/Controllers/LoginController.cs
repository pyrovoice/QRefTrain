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
            ModelState.AddModelError("User.Name", QRefResources.Resource.Login_ErrorInformations);

            return View("Login", viewModel);
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendResetCode(string userMail)
        {
            // Get the mail in db
            User user = Dal.Instance.GetUserByMail(userMail);
            // If found, create a message with a special code, store this in the db, then send the mail
            if (user != null)
            {
                Random generator = new Random();
                Request r = Dal.Instance.CreateRequest(new Request()
                {
                    CreationDate = new DateTime(),
                    RequestType = RequestType.ResetPassword,
                    SecretCode = generator.Next(0, 999999).ToString("D6"),
                    User = user
                });
                SendMail(user, string.Format(QRefResources.Resource.ResetPassword_Mailbody, r.SecretCode));
            }

            // Regardless of the result, move to the validation code page
            return View("ResetCode");
        }

        [HttpPost]
        public ActionResult ResetCode(string newPassword, string confirmPassword, string resetCode)
        {
            if (!newPassword.Equals(confirmPassword))
            {
                @ViewBag.Error = "Please specify the same password in both entries.";
                return View("ResetCode");
            }
            Request request = Dal.Instance.GetRequestByCode(resetCode);
            if (request == null)
            {
                @ViewBag.Error = "Please verify that the code you entered is valid.";
                return View("ResetCode");
            }
            Dal.Instance.UpdateUserChangePassword(request.User, newPassword);
            return View("ResetPasswordSuccess");

        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAccount(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isAllValid = true;
                if (Dal.Instance.IsUsernameAlreadyInDB(registerViewModel.Name))
                {
                    ViewBag.UsernameAlreadyInDB = QRefResources.Resource.CreateAccount_ErrorUsernameAlreadyUser;
                    isAllValid = false;
                }
                if (Dal.Instance.IsMailAlreadyInDB(registerViewModel.Email))
                {
                    ViewBag.MailError = QRefResources.Resource.CreateAccount_ErrorMailAlreadyUser;
                    isAllValid = false;
                }

                if (isAllValid)
                {
                    User newUser = new User() { Name = registerViewModel.Name, Email = registerViewModel.Email, Password = registerViewModel.Password };
                    Dal.Instance.CreateUser(newUser);
                    newUser = Dal.Instance.GetUserByName(newUser.Name);
                    Request request = Dal.Instance.CreateRequest(new Request()
                    {
                        CreationDate = Dal.Instance.GetDBTime(),
                        RequestType = RequestType.EmailConfirmation,
                        SecretCode = Guid.NewGuid().ToString("N"),
                        User = newUser
                    });
                    // Send a mail with a nice message that link to Login controller, ConfirmEmail with data secretCode and request ID
                    // String format put the result of Url Action (the URL to confirmation method) in the messageQRefResources.Resource.CreateAccount_Mailbody
                    string s = QRefResources.Resource.CreateAccount_Mailbody + "<a href=\'{0}\'>{1}</a>.";
                    string linkString = Request.Url.Authority + Url.Action("ConfirmEmail", "Login", new { Code = request.SecretCode, RequestId = request.Id });
                    string finalString = string.Format(s, linkString, QRefResources.Resource.Link);
                    finalString = "<html><body>" + finalString + "</body></html>";
                    //finalString = "<html><body><a href=\'http://quidditchreftraining.azurewebsites.net'>test</a></body></html>";
                    if (!SendMail(newUser, finalString))
                    {
                        Dal.Instance.DeleteUser(newUser);
                        ViewBag.MailError = QRefResources.Resource.CreateAccount_ErrorMailInnaccessible;
                        return View("CreateAccount", registerViewModel);
                    }
                    return View("CreateAccountConfirmation");
                }
            }
            return View("CreateAccount", registerViewModel);
        }

        public ActionResult ConfirmEmail(string Code, int RequestId)
        {
            //Get request corresponding to data
            Request request = Dal.Instance.GetRequestByCodeAndId(Code, RequestId);

            // If found, update the account and delete the request
            if (request != null)
            {
                var instance = Dal.Instance;
                instance.UpdateUserConfirmMail(request.User);
                instance.DeleteRequest(request);
            }

            // Regardless of the result, move to result page
            return View("ValidateAccountConfirmation");
        }

        public ActionResult Disconnect()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        private bool SendMail(User user, string body)
        {
            using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("qreftrain@gmail.com");
                mail.To.Add(user.Email);
                mail.Subject = "QuidditchRefTraining Subscription";
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("qreftrain@gmail.com", "uypcjrqukouxmzws");
                SmtpServer.EnableSsl = true;
                try
                {
                    Dal.Instance.CreateLog(new Log()
                    {
                        LogText = "Registeration mail sent with body " + body + "using adress " + mail.From,
                        LogTime = DateTime.Now,
                        UserId = user.Id
                    });
                    SmtpServer.Send(mail);
                }
                catch (Exception e)
                {
                    Dal.Instance.CreateLog(new Log()
                    {
                        LogText = "Error when sending mail to : " + user.Email + ", with body : " + body + "\nException : " + e.Message,
                        LogTime = new DateTime(),
                        UserId = user.Id
                    });
                    return false;
                }
                return true;
            }
        }
    }
}