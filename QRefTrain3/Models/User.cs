using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QRefTrain3.Models
{
    public enum UserRole { Default, Admin, Moderator}

    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(30, MinimumLength = 4, ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errorname")]
        [Display(Name = "Login_Name", ResourceType = typeof(QRefResources.Resource))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Required")]
        [StringLength(32, MinimumLength = 6, ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errorpassword")]
        [Display(Name = "Login_Password", ResourceType = typeof(QRefResources.Resource))]
        public string Password { get; set; }
        [Required(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(QRefResources.Resource), ErrorMessageResourceName = "Login_Errormail")]
        [Display(Name = "Login_Email", ResourceType = typeof(QRefResources.Resource))]
        public string Email { get; set; }
        [Required]
        public bool IsEmailConfirmed { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime RegisterationDate { get; set; }
        public UserRole UserRole { get; set; }
    }
}