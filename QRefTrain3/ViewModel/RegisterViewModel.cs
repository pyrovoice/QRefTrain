﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel
{
    public class RegisterViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(30, MinimumLength = 4, ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Login_Errorname")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(20, MinimumLength = 6, ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Login_Errorpassword")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Login_Errorconfirmationpassword")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Login_Errormail")]
        public string Email { get; set; }
    }
}