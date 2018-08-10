using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Must be 4 character long")]
        [Display(Name = "Login_Name", ResourceType = typeof(Resource.Resource))]
        public string Name { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "Must be 6 character long")]
        [Display(Name = "Login_Password", ResourceType = typeof(Resource.Resource))]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Login_Email", ResourceType = typeof(Resource.Resource))]
        public string Email { get; set; }
        [Required]
        public bool IsEmailConfirmed { get; set; }
    }
}