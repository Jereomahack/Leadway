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
    public class HODsController :BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HODs
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Hod.ToList());
        }


        [Authorize(Roles = "Admin")]
        // GET: HODs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HODs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,StaffNumber,Category")] HOD hOD)
        {
            if (ModelState.IsValid)
            {
                db.Hod.Add(hOD);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hOD);
        }

        // GET: HODs/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HOD hOD = db.Hod.Find(id);
            if (hOD == null)
            {
                return HttpNotFound();
            }
            return View(hOD);
        }

        // POST: HODs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,StaffNumber,Category")] HOD hOD)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hOD).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hOD);
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
