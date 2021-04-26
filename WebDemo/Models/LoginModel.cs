using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebDemo.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "Please enter your UserName")]
        public string UserName { set; get; }
        [Display(Name = "Password")]

        [Required(ErrorMessage = "Please enter your Password")]

        public string Password { set; get; }

    }
}