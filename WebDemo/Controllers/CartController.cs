using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebDemo.Dao;
using WebDemo.EF;
using WebDemo.Models;
using PayPal.Api;
namespace WebDemo.Controllers
{
    public class CartController : Controller
    {
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if(cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public JsonResult Update (string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach(var item in sessionCart){

                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                if (jsonItem != null)
                {
                    item.Quatity = jsonItem.Quatity;

                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        public JsonResult Delete(long id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }


        public ActionResult AddItem (long productId, int quantity)
        {   
            var product = new ProductDao1().ViewDetail(productId);
            var cart = Session[CartSession];
            if(cart != null)
            {
                
                var list = (List<CartItem>)cart;
                if(list.Exists(x => x.Product.ID == productId))
                {
                foreach(var item in list)
                {   
                    if(item.Product.ID == productId)
                    {
                        item.Quatity += quantity;
                    }
                }

                }
                else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quatity = quantity;
                    list.Add(item);
                }

            }

            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quatity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                Session[CartSession] = list;
            }

            return RedirectToAction("Index");
          
        }
        [HttpGet]

        public ActionResult Payment()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipName, string mobile, string address, string email, string productname)
        {
            
            var order = new WebDemo.EF.Order();
            order.CreateDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;
            

            var id = new OrderDao().Insert(order);
            var cart = (List<CartItem>)Session[CartSession];
            var detailDao = new OrderDetailDao();
            decimal total = 0;
            try
            {
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();

                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Product = item.Product.Name;
                    orderDetail.Quantity = item.Quatity;
                    orderDetail.Total = (item.Product.Price.GetValueOrDefault(0) * item.Quatity);
                    detailDao.Insert(orderDetail);
                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quatity);


                }
            

                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{ProductName}}", productname);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "New orders from OnlineShop", content);
                new MailHelper().SendMail(toEmail, "New orders from OnlineShop", content);
            }   
            catch(Exception ex)
            {
                return Redirect("/error-payment");
            }
            return Redirect("/success");
        }

        public ActionResult Success()
        {
            return View();
        }

        // Work with PayPal Payment
        private Payment payment;

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listItems = new ItemList() { items = new List<Item>() };
            List<CartItem> listCarts = (List<CartItem>)Session[CartSession];
            foreach( var cart in listCarts)
            {
                listItems.items.Add(new Item() 
                {
                    name = cart.Product.Name,
                    currency = "USD",
                    price = cart.Product.Price.ToString(),
                    quantity = cart.Quatity.ToString(),
                    sku = "sku"
                });
            }

            var payer = new Payer() { payment_method = "paypal" };
            //Do the configuaration RedirectURLs here with redirectURLs objects
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };
            //Create details object 
            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = listCarts.Sum(x => x.Quatity * x.Product.Price).ToString()
            };

            //Create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(), //tax+shipping+subtotal
                details = details
            };

            //Create transaction  
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description= "Tuan Testing transaction description",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);
        }

        //Create Excecute Payment Method 
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment() {id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        public ActionResult PaymentWithPayPal()
        {
            //Getting context form the paypal bases on clientId and clientSecret for payment
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //Creatring a payment
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //Get link returned from paypal response to create function
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;
                    var order = new WebDemo.EF.Order();
                    order.CreateDate = DateTime.Now;
                    order.ShipName = createdPayment.id;
                    var id = new OrderDao().Insert(order);
                    var cart = (List<CartItem>)Session[CartSession];
                    var detailDao = new OrderDetailDao();
                    decimal total = 0;
                    foreach (var item in cart)
                    {
                            var orderDetail = new OrderDetail();

                            orderDetail.ProductID = item.Product.ID;
                            orderDetail.OrderID = id;
                            orderDetail.Price = item.Product.Price;
                            orderDetail.Product = item.Product.Name;
                            orderDetail.Quantity = item.Quatity;
                            orderDetail.Total = (item.Product.Price.GetValueOrDefault(0) * item.Quatity);
                            detailDao.Insert(orderDetail);
                            total += (item.Product.Price.GetValueOrDefault(0) * item.Quatity);
                    }
                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }

                else
                {
                    //This one will be executed when we have received all the payment params form previous call 
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Failure");
                    }
                }
            }
            catch (Exception ex)
            {
                PaypalLogger.Log("Error: " + ex.Message);
                return View("Failure");
            }
            return View("Success");
        }
    }
}