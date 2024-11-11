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
    public class TransportMembersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TransportMembers
        public ActionResult Index()
        {
            return View(db.TransportMembers.ToList());
        }

        // GET: TransportMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportMember transportMember = db.TransportMembers.Find(id);
            if (transportMember == null)
            {
                return HttpNotFound();
            }
            return View(transportMember);
        }

        // GET: TransportMembers/Create
        public ActionResult Create(int id)
        {
            //finding student
            Student student = db.Students.Find(id);

            //inserting student details into viewbag to display to the view
            //student full Name
            var Surname = student.Surname;
            var otherName = student.OtherName;
            ViewBag.StudentName = Surname + " " + otherName;
            ViewBag.StudentNumber = student.StudentNumber;
            ViewBag.Class = student.Class;
            ViewBag.PhoneNumber = student.PhoneNumber;

            ViewBag.Driver = new SelectList(db.Transports, "Id", "DriverName", "Select Driver");
            ViewBag.Route = new SelectList(db.Routes, "Id", "RouteName", "Select Route");

            //passing data into TempDate to save into database
            TempData["StudentName"] = ViewBag.StudentName;
            TempData["StudentNumber"] = ViewBag.StudentNumber;
            TempData["Class"] = ViewBag.Class;
            TempData["PhoneNumber"] = ViewBag.PhoneNumber;

            return View();
        }

        // POST: TransportMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentNumber,StudentName,Class,PhoneNumber,Route,Driver")] TransportMember transportMember, int id)
        {
            //getting the user
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            //Saving TempData into Database 
            transportMember.StudentName = Convert.ToString(TempData["StudentName"]);
            transportMember.StudentNumber = Convert.ToString(TempData["StudentNumber"]);
            transportMember.Class = Convert.ToString(TempData["Class"]);
            transportMember.PhoneNumber = Convert.ToString(TempData["PhoneNumber"]);

            transportMember.DateRecorded = Convert.ToString(DateTime.Now.Date);
            transportMember.RegisteredBy = user.FullName;

            //converting Route and Driver Value
            int RouteId = Convert.ToInt32(transportMember.Route);
            int DriverId = Convert.ToInt32(transportMember.Driver);
            Route route = db.Routes.Find(RouteId);
            Transport driver = db.Transports.Find(DriverId);

            transportMember.Route = route.RouteName;
            transportMember.Driver = driver.DriverName;

            //saving records to database
            db.TransportMembers.Add(transportMember);
            db.SaveChanges();

            ViewBag.Driver = new SelectList(db.Transports, "Id", "DriverName");
            ViewBag.Route = new SelectList(db.Routes, "Id", "RouteName");
            return RedirectToAction("Index");
            
           
        }

        // GET: TransportMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportMember transportMember = db.TransportMembers.Find(id);
            if (transportMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.Driver = new SelectList(db.Transports, "Id", "DriverName");
            ViewBag.Route = new SelectList(db.Routes, "Id", "RouteName");
            return View(transportMember);
        }

        // POST: TransportMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentNumber,StudentName,Class,PhoneNumber,Route,Driver")] TransportMember transportMember)
        {
            //getting the user
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            transportMember.DateRecorded = Convert.ToString(DateTime.Now.Date);
            transportMember.RegisteredBy = user.FullName;

            //converting Route and Driver Value
            int RouteId = Convert.ToInt32(transportMember.Route);
            int DriverId = Convert.ToInt32(transportMember.Driver);
            Route route = db.Routes.Find(RouteId);
            Transport driver = db.Transports.Find(DriverId);

            transportMember.Route = route.RouteName;
            transportMember.Driver = driver.DriverName;

            if (ModelState.IsValid)
            {
                db.Entry(transportMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Driver = new SelectList(db.Transports, "Id", "DriverName");
            ViewBag.Route = new SelectList(db.Routes, "Id", "RouteName");
            return View(transportMember);
        }

        // GET: TransportMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransportMember transportMember = db.TransportMembers.Find(id);
            if (transportMember == null)
            {
                return HttpNotFound();
            }
            return View(transportMember);
        }

        // POST: TransportMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TransportMember transportMember = db.TransportMembers.Find(id);
            db.TransportMembers.Remove(transportMember);
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
