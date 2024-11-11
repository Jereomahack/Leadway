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
    public class InventoriesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inventories
        [Authorize(Roles ="Store Keeper")]
        public ActionResult Index()
        {
            return View(db.Inventories.ToList());
        }

        // GET: Inventories/Details/5
        [Authorize(Roles = "Store Keeper")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        [Authorize(Roles = "Store Keeper")]
        public ActionResult Create()
        {
            ViewBag.Product = new SelectList(db.Products, "Id", "ProductName");
            ViewBag.Vendor = new SelectList(db.Vendors, "Id", "VendorName");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Store Keeper")]
        public ActionResult Create([Bind(Include = "Id,VendorName,UnitPrice,Quantity,TotalPrice,Payment,EnteredBy,DateRecorded,Month,Year")] Inventory inventory, string Vendor, string Product)
        {

            //saving Month and Year
            inventory.Month = Convert.ToString(DateTime.Now.Month);

            //getting accademic Year
            GlobalSettings year = db.GlobalSettings.FirstOrDefault(y=>y.Name== "Session");
            inventory.AcademicYear = year.Value;
            inventory.DateRecorded = DateTime.Now;



            //getting the user
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            inventory.EnteredBy = user.FullName;

            //saving Vendor 
            int vendorId = Convert.ToInt32(Vendor);
            Vendor vendor = db.Vendors.Find(vendorId);
            inventory.VendorName = vendor.VendorName;

            //saving Product Name
            int productId = Convert.ToInt32(Product);
            Products prod = db.Products.Find(productId);
            inventory.Product = prod.ProductName;

            //getting total Price
            inventory.TotalPrice = inventory.Quantity * inventory.UnitPrice;

            //getting Term
            GlobalSettings term = db.GlobalSettings.FirstOrDefault(t=>t.Name=="Term");
            inventory.Term = term.Value;

            //saving records to Database 
            db.Inventories.Add(inventory);
            db.SaveChanges();

            //Searching store to know if this product exist
            Store store = db.Stores.FirstOrDefault(s => s.Product == inventory.Product);

            if (store == null)
            {
                Store stor = new Store()
                {
                    Product = inventory.Product,
                    Quantity = inventory.Quantity,
                    DateStored = inventory.DateRecorded,
                    StoredBy = inventory.EnteredBy,
                };
                //saving records to Database 
                db.Stores.Add(stor);
                db.SaveChanges();
            }
            else
            {
                Store updateStore = db.Stores.FirstOrDefault(s=>s.Product==inventory.Product);
                updateStore.Quantity += inventory.Quantity;

                //Updating Records of the Database
                db.Entry(updateStore).State = EntityState.Modified;
                db.SaveChanges();
            }
            

            return RedirectToAction("Index");


        }

        // GET: Inventories/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Inventory inventory = db.Inventories.Find(id);
        //    if (inventory == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(inventory);
        //}

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,VendorName,UnitPrice,Quantity,TotalPrice,Payment,EnteredBy,DateRecorded,Month,Year")] Inventory inventory)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(inventory).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(inventory);
        //}

        // GET: Inventories/Delete/5
        [Authorize(Roles = "Store Keeper")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventories.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Store Keeper")]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
            db.SaveChanges();

            //searching store DB
            Store store = db.Stores.FirstOrDefault(s=>s.Product==inventory.Product);
            store.Quantity -= inventory.Quantity;

            //updating Store DB after Deleting
            db.Entry(store).State = EntityState.Modified;
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
