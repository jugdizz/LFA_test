using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebApplication_DbFirst.Models;

namespace WebApplication_DbFirst.Controllers
{
    public class productController : Controller
    {
        private Entities db = new Entities();

        // GET: product
        public ActionResult Index()
        {
            var tbl_product = db.tbl_product.Include(t => t.tbl_category);
            return View(tbl_product.ToList());
        }

        // GET: product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_product tbl_product = db.tbl_product.Find(id);
            if (tbl_product == null)
            {
                return HttpNotFound();
            }
            return View(tbl_product);
        }

        // GET: product/Create
        public ActionResult Create()
        {
            ViewBag.Category_Id = new SelectList(db.tbl_category, "Category_Id", "Name");
            return View();
        }

        // POST: product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_Id,Name,Description,Image,Price,Category_Id")] tbl_product tbl_product,HttpPostedFileBase productImage)
        {
            if (ModelState.IsValid)
            {
                if (productImage != null && productImage.ContentLength>0)
                    try
                    {
                        string PathToFolder = Server.MapPath("~/Images");
                        string fileName = Path.GetFileName(productImage.FileName);
                        string path = Path.Combine(PathToFolder, fileName);

                        productImage.SaveAs(path);
                        tbl_product.Image = productImage.FileName;

                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "ERROR" + ex.Message.ToString();
                    }
                db.tbl_product.Add(tbl_product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Category_Id = new SelectList(db.tbl_category, "Category_Id", "Name", tbl_product.Category_Id);
            return View(tbl_product);
        }

        // GET: product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_product tbl_product = db.tbl_product.Find(id);
            if (tbl_product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_Id = new SelectList(db.tbl_category, "Category_Id", "Name", tbl_product.Category_Id);
            return View(tbl_product);
        }

        // POST: product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_Id,Name,Description,Image,Price,Category_Id")] tbl_product tbl_product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category_Id = new SelectList(db.tbl_category, "Category_Id", "Name", tbl_product.Category_Id);
            return View(tbl_product);
        }

        // GET: product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_product tbl_product = db.tbl_product.Find(id);
            if (tbl_product == null)
            {
                return HttpNotFound();
            }
            return View(tbl_product);
        }

        // POST: product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_product tbl_product = db.tbl_product.Find(id);
            db.tbl_product.Remove(tbl_product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
