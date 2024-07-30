using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using ShopBanGiay.Models;

namespace ShopBanGiay.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Admin/Products
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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase imageFile)
        {
            //db.Products.Add(p);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    if (imageFile.ContentLength > 2000000)
                    {
                        ModelState.AddModelError("Image", "Kích thước file phải nhỏ hơn 2MB.");
                        return View();
                    }

                    var allowEx = new[] { ".jpg", ".png" };
                    var fileEx = Path.GetExtension(imageFile.FileName).ToLower();
                    if (!allowEx.Contains(fileEx))
                    {
                        ModelState.AddModelError("Image", "Chỉ chấp nhận file ảnh jpg hoặc png.");
                        return View();
                    }

                    product.Image = "";
                    db.Products.Add(product);
                    db.SaveChanges();

                    Product pro = db.Products.ToList().Last();

                    var fileName = pro.Id.ToString() + fileEx;
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    imageFile.SaveAs(path);

                    pro.Image = fileName;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    product.Image = "";
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            Product pro = db.Products.Where(row => row.Id == id).FirstOrDefault();
            return View(pro);
        }
        [HttpPost]
        public ActionResult Edit(Product pro)
        {
            Product product = db.Products.Where(row => row.Id == pro.Id).FirstOrDefault();
            product.Name = pro.Name;
            product.Price = pro.Price;
            product.Image = pro.Image;
            product.BrandId = pro.BrandId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            Product pro = db.Products.Where(row => row.Id == id).FirstOrDefault();
            return View(pro);
        }
        [HttpPost]
        public ActionResult Delete(int id, Product pro)
        {
            Product product = db.Products.Where(row => row.Id == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}