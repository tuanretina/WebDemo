using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.Dao;
using WebDemo.EF;

namespace WebDemo.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var order = new OrderDao().ViewDetail(id);
            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                var dao = new OrderDao();
                var result = dao.Update(order);
                if (result)
                {
                    return RedirectToAction("Index", "Order");
                }
                else
                {
                    ModelState.AddModelError("User", "Update Failed!");
                }
            }
            return View("Index");
        }

        [HttpDelete]

        public ActionResult Delete(int id)
        {
            new OrderDao().Delete(id);
            return RedirectToAction("Index");
        }
        // GET: Admin/Order
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
    
            var dao = new OrderDao();
            var model = dao.ListAllPaging(searchString,  page,  pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

    }
}