using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDemo.EF;
using PagedList;
using System.Net;
using Sitecore.FakeDb;
using WebDemo.Dao;
using System.Data.Entity;

namespace WebDemo.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        OnlineShop db = new OnlineShop();
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        public ActionResult Add(HttpPostedFileBase file, Product product)
        {

       
            string filename = Path.GetFileName(file.FileName);
            string _filename = DateTime.Now.ToString("yymmssfff") + filename;
            string extension = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("/Image/"), _filename);
            product.Image = "/Image/" + _filename;

            if(extension.ToLower()==".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
            {
                if(file.ContentLength <= 100000)
                {
                    db.Products.Add(product);
                    if (db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        ViewBag.msg = "Product Added";
                        ModelState.Clear();

                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.msg = "File Size must be Equal or less than 1mb";
                }
                
            }
            else
            {
                ViewBag.msg = "Inavlid File Type";
            }
           

            return View();
        }
        [HttpGet]
        public ActionResult View(int id)
        {
            Product product = new Product();
            using (OnlineShop db = new OnlineShop())
            {
                product = db.Products.Where(x => x.ID == id).FirstOrDefault();
            }
            return View(product);

        }

        [HttpGet]
        public ActionResult Edit(int?id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var product = db.Products.Find(id);
            SetViewBag(product.CategoryID);
            Session["imgPath"] = product.Image;
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
           
        }

        [HttpPost]
        public ActionResult Edit(HttpPostedFileBase file, Product product)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string _filename = DateTime.Now.ToString("yymmssfff") + filename;
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/Image/"), _filename);
                    product.Image = "/Image/" + _filename;

                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (file.ContentLength <= 100000)
                        {
                            db.Entry(product).State = EntityState.Modified;
                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString()); 
                            if (db.SaveChanges() > 0)
                            {
                                file.SaveAs(path);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }
                                TempData["msg"] = "Data Updated";
                                return RedirectToAction("Index");

                            }
                        }
                        else
                        {
                            ViewBag.msg = "File Size must be Equal or less than 1mb";
                        }

                    }
                    else
                    {
                        ViewBag.msg = "Inavlid File Type";
                    }
                }
                else
                {
                    product.Image = Session["imgPath"].ToString();
                    db.Entry(product).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        TempData["msg"] = "Data Updated";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View("Index");
        }
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new ProductDao().Delete(id);
            return RedirectToAction("Index");
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }
    }
}