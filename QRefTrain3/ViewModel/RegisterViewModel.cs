using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class RegisterViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(30, MinimumLength = 4, ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errorname")]
        [Display(Name = "Login_Name", ResourceType = typeof(QRefResources.Resource))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errorpassword")]
        [Display(Name = "Login_Password", ResourceType = typeof(QRefResources.Resource))]
        public string Password { get; set; }
        [Compare("Password", ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errorconfirmationpassword")]
        [Display(Name = "CreateAccount_Confirmpassword", ResourceType = typeof(QRefResources.Resource))]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errormail")]
        [Display(Name = "Login_Email", ResourceType = typeof(QRefResources.Resource))]
        public string Email { get; set; }
    }
}