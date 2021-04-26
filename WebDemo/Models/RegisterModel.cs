using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebDemo.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { set; get; }
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "Please enter your UserName")]
        
        public string UserName { set; get; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please enter your Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length is at least 6 characters!")]

        public string Password { set; get; }
        [Display(Name = "CofirmPassword")]
        [Compare("Password", ErrorMessage = "Password was wrong!")]
        public string CofirmPassword { set; get; }
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Please enter your Name")]
        public string Name { set; get; }
        [Display(Name = "Address")]
        public string Address { set; get; }
        [Required(ErrorMessage = "Please enter your Email")]
        [Display(Name = "Email")]
        public string Email { set; get; }
        [Display(Name = "Phone")]
        public string Phone { set; get; }

    }
}