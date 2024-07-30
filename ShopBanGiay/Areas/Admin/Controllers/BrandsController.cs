using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanGiay.Models;

namespace ShopBanGiay.Areas.Admin.Controllers
{
    public class BrandsController : Controller
    {
        // GET: Admin/Brands
        public ActionResult Index()
        {
            DBContext db = new DBContext();
            List<Brand> brands = db.Brands.ToList();
            return View(brands);
        }
    }
}