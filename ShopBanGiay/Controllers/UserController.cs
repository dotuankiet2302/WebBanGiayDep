using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanGiay.Models;
using BCrypt;

namespace ShopBanGiay.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user != null)
            {
                DBContext db = new DBContext();
                User myUser = db.User.Where(u => u.UserName == user.UserName).FirstOrDefault();
                if (myUser != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(user.Password, myUser.Password))
                    {
                        HttpCookie authCookie = new HttpCookie("auth", myUser.UserName);
                        HttpCookie roleCookie = new HttpCookie("role", myUser.Role);
                        Response.Cookies.Add(authCookie);
                        Response.Cookies.Add(roleCookie);
                        if (roleCookie.Value == "admin")
                        {
                            return RedirectToAction("Index", "Home", new { area = "admin" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("Password", "Invalid Username or Password !!!");
            }
            return View();
        }

        public ActionResult Logout()
        {
            HttpCookie authcookie = new HttpCookie("auth");
            authcookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(authcookie);

            HttpCookie roleCookie = new HttpCookie("role");
            roleCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(roleCookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user, string retypePassword)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (user.Password != retypePassword)
            {
                ModelState.AddModelError("retypePassword", "Passwords do not match.");
                return View();
            }

            DBContext db = new DBContext();
            User myUser = db.User.Where(u => u.UserName == user.UserName).FirstOrDefault();
            if (myUser != null)
            {
                ModelState.AddModelError("UserName", "UserName already exist.");
                return View();
            }

            myUser = db.User.Where(u => u.EmailAddress == user.EmailAddress).FirstOrDefault();
            if (myUser != null)
            {
                ModelState.AddModelError("EmailAddress", "EmailAddress already exist.");
                return View();
            }

            myUser = new User();
            myUser.UserName = user.UserName;
            myUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            myUser.EmailAddress = user.EmailAddress;
            myUser.Role = "user";
            db.User.Add(myUser);
            db.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}