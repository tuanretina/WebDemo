using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDemo.Common;
using WebDemo.EF;

namespace WebDemo.Dao
{
    public class UserDao
    {
        OnlineShop db = null;
        public UserDao()
        {
            db = new OnlineShop();
        }

        public long Insert(User entity)
        {

            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;

        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);

                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.GroupID = entity.GroupID;
                user.Name = entity.Name;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.CreateBy = entity.CreateBy;
                user.CreateDate = DateTime.Now;
                user.UserName = entity.UserName;
                user.ModifedBy = entity.ModifedBy;
                user.ModifiedDate = DateTime.Now;
                user.Status = entity.Status;
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<User> ListAllPaging(string seacrhString, int page, int pageSize)
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(seacrhString))
            {
                model = model.Where(x => x.UserName.Contains(seacrhString) || x.Email.Contains(seacrhString) || x.Name.Contains(seacrhString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }

        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }

        public int Login(string userName, string password)
        {

            var result = db.Users.SingleOrDefault(x => x.UserName == userName && x.Password == password);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == password)
                        return 1;
                    else
                        return -2;
                }
            }
        }
        public List<string> GetListCredential(string userName)
        {
            var user = db.Users.Single(x => x.UserName == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new 
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });

            return data.Select(x => x.RoleID).ToList();
        }
        public int Login(string userName, string password, bool isLoginAdmin = false)
        {

            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (isLoginAdmin = true)
                {
                    if (result.GroupID == CommonConstants1.ADMIN_GROUP || result.GroupID == CommonConstants1.MOD_GROUP)
                    {
                        if (result.Status == false)
                        {
                            return -1;
                        }


                        else
                        {
                            if (result.Password == password)
                                return 1;
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }

                }

                else
                {
                    if (result.Status == false)
                    {
                        return -1;
                    }


                    else
                    {
                        if (result.Password == password)
                            return 1;
                        else
                            return -2;
                    }
                }
            }
        }



        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }

        public bool CheckUserName(string userName)
        {
            return db.Users.Count(x => x.UserName == userName) > 0;

        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
    }
}