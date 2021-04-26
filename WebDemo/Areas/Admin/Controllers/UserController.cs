using WebDemo.Dao;
using WebDemo.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebDemo.Common;

namespace WebDemo.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        OnlineShop db = new OnlineShop();
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString,int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]

        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_USER")]

        public ActionResult Edit(int id)
        {
            var user = new UserDao().ViewDetail(id);
            SetViewBag(user.GroupID);
            return View(user);
        }
        [HttpPost]
        public ActionResult Create(User user)
        {
            if(ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(user.UserName))
                {
                    return View("Err");
                }
                else
                {
                    long id = dao.Insert(user);
                    if (id > 0)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Add new user failded!");
                    }
                }
               
            }
            return View("Index"); 
        }
        public ActionResult Err()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Update(user);
                if (result)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("User", "Update Failed!");
                }
            }
      
            return View("Index");
        }
        [HttpDelete]
        public ActionResult Delete (int id)
        {
            new UserDao().Delete(id);
            return RedirectToAction("Index");
        }

        public void SetViewBag(string selectedId = null)
        {
            var dao = new GroupDao();
            ViewBag.GroupID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}