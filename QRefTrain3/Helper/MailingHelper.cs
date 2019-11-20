using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace QRefTrain3.Helper
{
    public class MailingHelper
    {
        public static bool SendMail(User user, string subject, string body)
        {
            using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
            {
                var mail = new MailMessage();
                mail.From = new MailAddress("qreftrain@gmail.com");
                mail.To.Add(user.Email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("qreftrain@gmail.com", "uypcjrqukouxmzws");
                SmtpServer.EnableSsl = true;
                try
                {
                    SmtpServer.Send(mail);
                    Dal.Instance.CreateLog(new Log(LogLevel.INFO, "Registeration mail sent with body " + body + "using adress " + mail.From, DateTime.Now, user.Id));
                }
                catch (Exception e)
                {
                    Dal.Instance.CreateLog(new Log(LogLevel.ERROR, "Error when sending mail to : " + user.Email + ", with body : " + body + "\nException : " + e.Message, DateTime.Now, user.Id));
                    return false;
                }
                return true;
            }
        }

        public static void SendMailToQuestionSuiteOwner(Result resultToValidate)
        {
            String body = "User " + resultToValidate.User.Name + " completed your exam " + resultToValidate.QuestionSuite.Name + "\nResult: " + resultToValidate.GetNumberGoodAnswers() + "/" + resultToValidate.QuestionsAsked.Count + "\nTime: " + Dal.Instance.GetDBTime();
            Helper.MailingHelper.SendMail(resultToValidate.QuestionSuite.Owner, "User " + resultToValidate.User.Name + " completed your exam " + resultToValidate.QuestionSuite.Name, body);
        }
    }
}