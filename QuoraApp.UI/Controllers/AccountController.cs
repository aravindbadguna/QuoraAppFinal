using QuoraApp.ServiceLayer;
using QuoraApp.UI.CustomFilters;
using QuoraApp.ViewModels;
using System;
using System.Web.Mvc;


namespace QuoraApp.UI.Controllers
{
    public class AccountController : Controller
    {
        IUsersService us;
        public AccountController(IUsersService us)
        {
            this.us = us;
        }
        public ActionResult Register()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                int uid = this.us.InsertUser(rvm);
                Session["CurrentUserID"] = uid;
                Session["CurrentUserName"] = rvm.Name;
                Session["CurrentUserEmail"] = rvm.Email;
                Session["CurrentUserPassword"] = rvm.Password;
                Session["CurrentUserIsAdmin"] = false;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("error", "Invalid Data");
                return View();
            }           
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginViewModel lvm = new LoginViewModel();

            return View(lvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                UserViewModel uvm = this.us.GetUsersByEmailAndPassword(lvm.Email, lvm.Password);
                if(uvm != null)
                {
                    Session["CurrentUserID"] = uvm.UserID;
                    Session["CurrentUserName"] = uvm.Name;
                    Session["CurrentUserEmail"] = uvm.Email;
                    Session["CurrentUserPassword"] = uvm.Password;
                    Session["CurrentUserIsAdmin"] = uvm.IsAdmin;

                    if (uvm.IsAdmin)
                    {
                        return RedirectToRoute(new {area="Admin",controller="AdminHome",action="Index"});
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("error", "Invalid Email/Password");
                    return View(lvm);
                }
            }
            else
            {
                ModelState.AddModelError("error", "Invalid Data");
                return View(lvm);
            }           
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        [UseAuthorizationFilterAttribute]
        public ActionResult ChangeProfile()
        {
            int uid = Convert.ToInt32(Session["CurrentUserID"]);
            UserViewModel uvm = this.us.GetUsersByUserID(uid);
            EditUserDetailsViewModel eudvm = new EditUserDetailsViewModel()
            {
                UserID = uvm.UserID,
                Name = uvm.Name,
                Email = uvm.Email,
                Mobile = uvm.Mobile
            };
            return View(eudvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UseAuthorizationFilterAttribute]
        public ActionResult ChangeProfile(EditUserDetailsViewModel eudvm)
        {
            if (ModelState.IsValid)
            {
                eudvm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.us.UpdateUserDetails(eudvm);
                Session["CurrentUserName"] = eudvm.Name;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View(eudvm);
            }
        }

        [HttpGet]
        [UseAuthorizationFilter]
        public ActionResult ChangePassword()
        {
            int uid = Convert.ToInt32(Session["CurrentUserID"]);
            UserViewModel uvm = this.us.GetUsersByUserID(uid);
            EditUserPasswordViewModel eupvm = new EditUserPasswordViewModel()
            {
                UserID = uvm.UserID,
                Email = uvm.Email,
                Password = "",
                ConfirmPassword = ""
            };
            return View(eupvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(EditUserPasswordViewModel eupvm)
        {
            if (ModelState.IsValid)
            {
                eupvm.UserID = Convert.ToInt32(Session["CurrentUserID"]);
                this.us.UpdateUserPassword(eupvm);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View(eupvm);
            }
        }
    }
}