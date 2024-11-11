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
    public class StudentTransistionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentTransistions
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.StudentTransistions.ToList());
        }

        // GET: StudentTransistions/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTransistion studentTransistion = db.StudentTransistions.Find(id);
            if (studentTransistion == null)
            {
                return HttpNotFound();
            }
            return View(studentTransistion);
        }

        // GET: StudentTransistions/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int id)
        {

            Student student = db.Students.Find(id);
            //saving student details into viewbag

            ViewBag.StudentNumber = student.StudentNumber;
            ViewBag.Surname = student.Surname;
            ViewBag.OtherName = student.OtherName;
            ViewBag.Class = student.Class;
            ViewBag.Gender = student.Gender;
            ViewBag.Category = student.Category;

            return View();
        }

        // POST: StudentTransistions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,StudentNumber,Surname,OtherName,Gender,Category,Class,DateLeft,Passport,RegisteredBy,DateRecorded")] StudentTransistion studentTransistion)
        {

            //getting the student
            Student student = db.Students.FirstOrDefault(s => s.StudentNumber == studentTransistion.StudentNumber);

            studentTransistion.DateRecorded = DateTime.Now;
            //getting
            Teachers staff = db.Teachers.FirstOrDefault(s => s.EmailAddress == User.Identity.Name);

            if (User.IsInRole("Admin"))
            {
                studentTransistion.RegisteredBy = "Admin";
            }
            else
            {
                studentTransistion.RegisteredBy = staff.Name;
            }

            student.Transistion = "Yes";

            //saving records into database
            db.StudentTransistions.Add(studentTransistion);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        // GET: StudentTransistions/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTransistion studentTransistion = db.StudentTransistions.Find(id);
            if (studentTransistion == null)
            {
                return HttpNotFound();
            }
            return View(studentTransistion);
        }

        // POST: StudentTransistions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,StudentNumber,Surname,OtherName,Gender,Category,Class,DateLeft,Passport,RegisteredBy,DateRecorded")] StudentTransistion studentTransistion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentTransistion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(studentTransistion);
        }

        // GET: StudentTransistions/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentTransistion studentTransistion = db.StudentTransistions.Find(id);
            if (studentTransistion == null)
            {
                return HttpNotFound();
            }
            return View(studentTransistion);
        }

        // POST: StudentTransistions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentTransistion studentTransistion = db.StudentTransistions.Find(id);
            db.StudentTransistions.Remove(studentTransistion);
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
