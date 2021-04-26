using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;
using WebDemo.ViewModel;

namespace WebDemo.Dao
{
    public class ProductDao1
    {
        OnlineShop db = null;
        public ProductDao1()
        {
            db = new OnlineShop();
        }
        public List<Product> ListNewProduct(int top)
        {
            return db.Products.OrderByDescending(x => x.CreatedDate).Take(top).ToList();

        }

        //public List<Product> ListByCategoryId(long categoryId)
        //{
        //    return db.Products.Where(x => x.CategoryID == categoryId).ToList(); 
        //}

        
        public List<ProductViewModel> ListByCategoryId(long categoryID, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.CategoryID == categoryID).Count();
            var model = from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        where a.CategoryID == categoryID
                        select new ProductViewModel()
                        {
                            CateMetaTitle = b.MetaTitile,
                            CateName = b.Name,
                            CreatedDate = a.CreatedDate,
                            ID = a.ID,
                            Images = a.Image,
                            Name = a.Name,
                            MetaTitle = a.MetaTitile,
                            Price = a.Price
                        };
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();

    
        }

        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Products.Where(x => x.Name.Contains(keyword)).Count();
            var model = (from a in db.Products
                        join b in db.ProductCategories
                        on a.CategoryID equals b.ID
                        where a.Name.Contains(keyword)
                        select new
                        {
                            CateMetaTitle = b.MetaTitile,
                            CateName = b.Name,
                            CreatedDate = a.CreatedDate,
                            ID = a.ID,
                            Images = a.Image,
                            Name = a.Name,
                            MetaTitle = a.MetaTitile,
                            Price = a.Price
                        }).AsEnumerable().Select(x => new ProductViewModel()
                        {
                            CateMetaTitle = x.MetaTitle,
                            CateName = x.Name,
                            CreatedDate = x.CreatedDate,
                            ID = x.ID,
                            Images = x.Images,
                            Name = x.Name,
                            MetaTitle = x.MetaTitle,
                            Price = x.Price
                        });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();


        }

        public List<Product> ListFeatureProduct(int top)
        {
            return db.Products.Where(x => x.Tophot != null && x.Tophot > DateTime.Now).OrderByDescending(x => x.CreatedDate).Take(top).ToList();
        }

        public List<string> ListName (string keyword)
        {
            return db.Products.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }

        public List<Product> ListRelatedProduct(long productId)
        {
            var product = db.Products.Find(productId);
            return db.Products.Where(x => x.ID != productId && x.CategoryID == product.CategoryID).ToList();
                
        }


        public Product ViewDetail(long id)
        {
            return db.Products.Find(id);
        }
    }
}