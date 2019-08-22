using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QRefTrain3.Models
{
    public enum RequestType { EmailConfirmation, ResetPassword }

    public class Request
    {

        public int Id { get; set; }
        [Required]
        public RequestType RequestType { get; set; }
        [Required]
        public virtual User User { get; set; }
        [Required]
        [StringLength(32)]
        public string SecretCode { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}