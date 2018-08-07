using System;
using System.Web;

namespace QRefTrain3.Helper
{

    public enum CookieNames
    {
        RequestedNGB
    }

    public class CookieHelper
    {
        /// <summary>
        /// Create a cookie or update its value in the given response.
        /// </summary>
        /// <param name="request">Required to check the cookies list</param>
        /// <param name="response">Required to update the cookie list in the response</param>
        /// <param name="name">Cookie's name</param>
        /// <param name="value">Cookie's value</param>
        /// <param name="dateTime">Cookie's expiration date</param>
        public static void UpdateCookie(HttpRequestBase request, HttpResponseBase response, CookieNames name, string value, DateTime dateTime)
        {
            HttpCookie cookie = request.Cookies[name.ToString()];
            if (cookie != null)
                cookie.Value = value;
            else
            {
                cookie = new HttpCookie(name.ToString())
                {
                    Value = value,
                    Expires = dateTime
                };
            }
            response.Cookies.Add(cookie);
        }
    }
}