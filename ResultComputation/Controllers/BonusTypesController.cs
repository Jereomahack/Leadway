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
    public class BonusTypesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BonusTypes
        public ActionResult Index()
        {
            return View(db.BonusTypes.ToList());
        }

        // GET: BonusTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BonusType bonusType = db.BonusTypes.Find(id);
            if (bonusType == null)
            {
                return HttpNotFound();
            }
            return View(bonusType);
        }

        // GET: BonusTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BonusTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BonusTypeName")] BonusType bonusType)
        {
            if (ModelState.IsValid)
            {
                db.BonusTypes.Add(bonusType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bonusType);
        }

        // GET: BonusTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BonusType bonusType = db.BonusTypes.Find(id);
            if (bonusType == null)
            {
                return HttpNotFound();
            }
            return View(bonusType);
        }

        // POST: BonusTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BonusTypeName")] BonusType bonusType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bonusType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bonusType);
        }

        // GET: BonusTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BonusType bonusType = db.BonusTypes.Find(id);
            if (bonusType == null)
            {
                return HttpNotFound();
            }
            return View(bonusType);
        }

        // POST: BonusTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BonusType bonusType = db.BonusTypes.Find(id);
            db.BonusTypes.Remove(bonusType);
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
