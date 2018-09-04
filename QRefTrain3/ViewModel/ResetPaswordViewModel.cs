using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class ResetPaswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(6, MinimumLength = 6, ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errorcode")]
        string SecretCode { get; set; }
        [Required(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errorpassword")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errorconfirmationpassword")]
        public string ConfirmPassword { get; set; }
    }
}