using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;
using PayPal.Api;

namespace WebDemo.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quatity { get; set; }
    }
}