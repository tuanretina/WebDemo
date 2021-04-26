using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.EF;
using WebDemo.ViewModel;

namespace WebDemo.Dao
{
    public class OrderDao
    {
        OnlineShop db = null;
        public OrderDao()
        {
            db = new OnlineShop();
        }
        public long Insert(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
            return order.ID;

        }

        public Order ViewDetail(int id)
        {
            return db.Orders.Find(id);
        }
        public bool Update(Order entity)
        {
            try
            {
                var order = db.Orders.Find(entity.ID);
                order.CreateDate = DateTime.Now;
                order.ShipName = entity.ShipName;
                order.ShipAddress = entity.ShipAddress;
                order.ShipEmail = entity.ShipEmail;
                order.ShipMobile = entity.ShipMobile;
                order.Status = entity.Status;
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<OrderViewModel> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<OrderViewModel> model;
            model = from a in db.Orders
                    join b in db.OrderDetails
                    on a.ID equals b.OrderID
                    join c in db.Products
                    on b.ProductID equals c.ID
                 



                    select new OrderViewModel()
                    {
                        ID = a.ID,
                        Total = b.Total,
                        ShipAddress = a.ShipAddress,
                        ShipName = a.ShipName,
                        ShipMobile = a.ShipMobile,
                        ShipEmail = a.ShipEmail,
                        Status = a.Status,
                        CreatedDate = a.CreateDate,
                        Price = c.Price,
                        Product = c.Name,
                        Quantity = b.Quantity
          


                    };
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.ShipName.Contains(searchString) || x.ShipAddress.Contains(searchString) || x.ShipMobile.Contains(searchString) || x.ShipEmail.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }


        //public IEnumerable<Order> ListAllPaging(string seacrhString, int page, int pageSize)
        //{
        //    IQueryable<Order> model = db.Orders;
        //    if (!string.IsNullOrEmpty(seacrhString))
        //    {
        //        model = model.Where(x => x.ShipName.Contains(seacrhString) || x.ShipName.Contains(seacrhString));
        //    }
        //    return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        //}
    }

}