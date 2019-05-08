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
                    Dal.Instance.CreateLog(new Log()
                    {
                        LogText = "Registeration mail sent with body " + body + "using adress " + mail.From,
                        LogTime = DateTime.Now,
                        UserId = user.Id
                    });
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