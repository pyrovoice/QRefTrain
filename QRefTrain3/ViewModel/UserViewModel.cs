using QRefTrain3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QRefTrain3.ViewModel

{
    public class UserViewModel
    {
        [Required]
        public User User { get; set; }
        public bool IsAutenthified { get; set; }
    }
}