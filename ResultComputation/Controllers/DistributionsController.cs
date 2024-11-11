using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LightWay.Models;

namespace LightWay.Controllers
{
    public class DistributionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Distributions
        [Authorize(Roles ="Store Keeper")]
        public ActionResult Index(int? Id)
        {
            Store store = db.Stores.Find(Id);
            if (store != null)
            {
                ViewBag.Quantity = "The Total Quantity of" + " " + store.Product + " " + " in store is" + " " + store.Quantity;
            }
            else
            {
                ViewBag.Quantity = "";
            }


            return View(db.Distributions.ToList());
        }

        // GET: Distributions/Details/5
        [Authorize(Roles = "Store Keeper")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Distribution distribution = db.Distributions.Find(id);
            if (distribution == null)
            {
                return HttpNotFound();
            }
            return View(distribution);
        }

        // GET: Distributions/Create
        [Authorize(Roles = "Store Keeper")]
        public ActionResult Create()
        {
            ViewBag.Receiver = new SelectList(db.Departments, "Id", "HOD");
            ViewBag.Item = new SelectList(db.Stores, "Id", "Product");
            return View();
        }

        // POST: Distributions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Store Keeper")]
        public ActionResult Create([Bind(Include = "Id,Receiver,Department,ItemName,Quantity,EnteredBy,DateRecorded")] Distribution distribution, string Receiver, string Item)
        {
            //Converting Item Id
            int itemId = Convert.ToInt32(Item);
            Store prod = db.Stores.Find(itemId);

            if (prod.Quantity > distribution.Quantity)
            {

                //getting Receiver and departments
                int receiverId = Convert.ToInt32(Receiver);
                Department receiver = db.Departments.Find(receiverId);

                //saving Receiver and Department 
                distribution.Receiver = receiver.HOD;
                distribution.Department = receiver.DepartmentName;

                //getting Item Name
                distribution.ItemName = prod.Product;

                //getting Academic Year
                GlobalSettings year = db.GlobalSettings.FirstOrDefault(y => y.Name == "Session");
                distribution.AcademicYear = year.Value;

                //getting Term
                GlobalSettings term = db.GlobalSettings.FirstOrDefault(y => y.Name == "Term");
                distribution.Term = term.Value;

                //getting user
                Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
                distribution.EnteredBy = user.FullName;

                //saving other records
                distribution.Month = Convert.ToString(DateTime.Now.Month);
                distribution.DateRecorded = DateTime.Now;

                //Saving Records Into DB
                db.Distributions.Add(distribution);
                db.SaveChanges();

                //updating Product Quantity in Store DB
                Store store = db.Stores.FirstOrDefault(s => s.Product == distribution.ItemName);
                store.Quantity -= distribution.Quantity;

                //update Store
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();

                
            }
            else
            {
                ViewBag.NoDetail = "The quantity in store is " + " "+ prod.Quantity;
                ViewBag.Receiver = new SelectList(db.Departments, "Id", "HOD");
                ViewBag.Item = new SelectList(db.Stores, "Id", "Product");
                return View();
            }

            int Id = prod.Id;
            return RedirectToAction("Index", new { Id });

        }

        // GET: Distributions/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Distribution distribution = db.Distributions.Find(id);
        //    if (distribution == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(distribution);
        //}

        // POST: Distributions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Receiver,Department,ItemName,Quantity,EnteredBy,DateRecorded")] Distribution distribution)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(distribution).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(distribution);
        //}

        // GET: Distributions/Delete/5
        [Authorize(Roles = "Store Keeper")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Distribution distribution = db.Distributions.Find(id);
            if (distribution == null)
            {
                return HttpNotFound();
            }
            return View(distribution);
        }

        // POST: Distributions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Store Keeper")]
        public ActionResult DeleteConfirmed(int id)
        {
            Distribution distribution = db.Distributions.Find(id);
            db.Distributions.Remove(distribution);
            db.SaveChanges();

            //Updating store after delete
            Store store = db.Stores.FirstOrDefault(s => s.Product == distribution.ItemName);
            store.Quantity += distribution.Quantity;

            //update Store
            db.Entry(store).State = EntityState.Modified;
            db.SaveChanges();

            int Id = store.Id;
            return RedirectToAction("Index", new { Id });
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
