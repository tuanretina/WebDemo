﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebDemo.Areas.Admin.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage =  "Please input username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please input username")]

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}