﻿using System;
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
    public class GradesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Grades
        [Authorize(Roles = "Admin,Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Index()
        {
            return View(db.Grades.ToList());
        }

        // GET: Grades/Details/5

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Grades grades = db.Grades.Find(id);
        //    if (grades == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(grades);
        //}

        // GET: Grades/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Grades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,GradeName,GradePoint,MarkFrom,MarkUpto,Note")] Grades grades)
        {
            if (ModelState.IsValid)
            {
                db.Grades.Add(grades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grades);
        }

        // GET: Grades/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grades grades = db.Grades.Find(id);
            if (grades == null)
            {
                return HttpNotFound();
            }
            return View(grades);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,GradeName,GradePoint,MarkFrom,MarkUpto,Note")] Grades grades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grades);
        }

        // GET: Grades/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grades grades = db.Grades.Find(id);
            if (grades == null)
            {
                return HttpNotFound();
            }
            return View(grades);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Grades grades = db.Grades.Find(id);
            db.Grades.Remove(grades);
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