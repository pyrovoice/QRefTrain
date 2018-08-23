using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public enum UserRole { Default, Admnin, Moderator}

    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(30, MinimumLength = 4, ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Login_Errorname")]
        [Display(Name = "Login_Name", ResourceType = typeof(Resource.Resource))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(32, MinimumLength = 6, ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Login_Errorpassword")]
        [Display(Name = "Login_Password", ResourceType = typeof(Resource.Resource))]
        public string Password { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "Login_Errormail")]
        [Display(Name = "Login_Email", ResourceType = typeof(Resource.Resource))]
        public string Email { get; set; }
        [Required]
        public bool IsEmailConfirmed { get; set; }
        public DateTime RegisterationDate { get; set; }
        public UserRole UserRole { get; set; }
    }
}