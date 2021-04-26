using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Common;
using WebDemo.Dao;
using WebDemo.EF;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Login(LoginModel model) {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, model.Password);
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "This account does not exist");

                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "This account has been locked");

                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Incorrect password");

                }

                else
                {
                    ModelState.AddModelError("", "Login Failed.");

                }
            }
            return View(model);


        }
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Username available!");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email available!");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Password = model.Password;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    model.Address = model.Address;
                    user.CreateDate = DateTime.Now;
                    user.Status = true;
                    var result = dao.Insert(user);
                    if(result > 0)
                    {
                        ViewBag.Success = "Sign Up Success";
                        model = new RegisterModel();

                    }
                    else
                    {
                        ModelState.AddModelError("", "Sign Up Failed");

                    }
                }
            }
            return View(model);
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new GroupDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}