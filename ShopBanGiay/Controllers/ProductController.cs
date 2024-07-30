using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanGiay.Models;

namespace ShopBanGiay.Controllers
{
    public class ProductController : Controller
    {
        // GET: Products
        DBContext db = new DBContext();
        public ActionResult Index(string search = "", string sortCol = "Price", string IconClass = "fa-sort-asc", int page = 1)
        {
            List<Product> products = db.Products.Where(row => row.Name.Contains(search)).ToList();
            ViewBag.Search = search;
            ViewBag.sortCol = sortCol;
            ViewBag.IconClass = IconClass;
            if (sortCol == "Price")
            {
                if (IconClass == "fa-sort-asc")
                {
                    products = products.OrderBy(row => row.Price).ToList();
                }
                else
                {
                    products = products.OrderByDescending(row => row.Price).ToList();
                }
            }
            //paging
            int NoOfRecordPerPage = 12;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(products.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            products = products.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            return View(products);
        }
        public ActionResult Detail(int id)
        {
            Product pro = db.Products.Where(row => row.Id == id).FirstOrDefault();
            return View(pro);
        }
    }
}