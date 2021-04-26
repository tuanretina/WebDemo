using WebDemo.Dao;
using System.Web.Mvc;
using WebDemo.Areas.Admin.Model;
using WebDemo.Common;

namespace WebDemo.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, model.Password, true);
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredential(model.UserName);
                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");    
                }else if (result == 0)
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

                else if (result == -3)
                {
                    ModelState.AddModelError("", "Your account does not have access to login");

                }

                else
                {
                    ModelState.AddModelError("", "Login Failed.");

                }
            }
            return View("Index");
           
        }
    }
}