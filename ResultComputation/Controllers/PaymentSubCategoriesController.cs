using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LightWay.Models;
using ResultComputation.Models;

namespace LightWay.Controllers
{
    public class PaymentSubCategoriesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaymentSubCategories
        public ActionResult Index()
        {

            return View( db.PaymentSubCategories.ToList());
        }



        public async Task<ActionResult> SubList(string Cat)
        {

            TempData["PCid"] = Cat;
            return View(await db.PaymentSubCategories.Where(t => t.PaymentCat == Cat).ToListAsync());
        }

        // GET: PaymentSubCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSubCategory paymentSubCategory = await db.PaymentSubCategories.FindAsync(id);
            if (paymentSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(paymentSubCategory);
        }

        // GET: PaymentSubCategories/Create
        public ActionResult Create()
        {
            ViewBag.PaymentCat = TempData["PCid"].ToString();

            return View();
        }

        // POST: PaymentSubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PaymentCat,SubCatName,Amount")] PaymentSubCategory paymentSubCategory)
        {
            if (ModelState.IsValid)
            {


                db.PaymentSubCategories.Add(paymentSubCategory);
                await db.SaveChangesAsync();

                return RedirectToAction("SubList", new { Cat = paymentSubCategory.PaymentCat });
            }

            return View(paymentSubCategory);
        }

        // GET: PaymentSubCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSubCategory paymentSubCategory = await db.PaymentSubCategories.FindAsync(id);
            if (paymentSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(paymentSubCategory);
        }

        // POST: PaymentSubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PaymentCat,SubCatName,Amount")] PaymentSubCategory paymentSubCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentSubCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paymentSubCategory);
        }

        // GET: PaymentSubCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSubCategory paymentSubCategory = await db.PaymentSubCategories.FindAsync(id);
            if (paymentSubCategory == null)
            {
                return HttpNotFound();
            }
            return View(paymentSubCategory);
        }

        // POST: PaymentSubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PaymentSubCategory paymentSubCategory = await db.PaymentSubCategories.FindAsync(id);
            db.PaymentSubCategories.Remove(paymentSubCategory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public ActionResult Fee(int? id)
        {
            return RedirectToAction("create", "Fees", new { id });
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
