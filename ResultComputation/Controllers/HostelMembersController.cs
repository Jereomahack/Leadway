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
    public class HostelMembersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HostelMembers
        public ActionResult Index()
        {
            return View(db.HostelMembers.ToList());
        }

        // GET: HostelMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HostelMember hostelMember = db.HostelMembers.Find(id);
            if (hostelMember == null)
            {
                return HttpNotFound();
            }
            return View(hostelMember);
        }

        // GET: HostelMembers/Create
        public ActionResult Create(int id)
        {
            //getting User
            Teachers teacher = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            //Finding Student
            Student student = db.Students.Find(id);
            //getting student full name
            var surname = student.Surname;
            var othername = student.OtherName;
            ViewBag.StudentName = surname +"  "+ othername;
            ViewBag.StudentNumber = student.StudentNumber;
            ViewBag.PhoneNumber = student.PhoneNumber;
            ViewBag.Gender = student.Gender;

            ViewBag.HostelName = new SelectList(db.Hostels, "Id", "HostelName");
            ViewBag.Class = new SelectList(db.Hostels, "Id", "Class");

            //passing data into TempDate to save into database
            TempData["StudentName"] = ViewBag.StudentName;
            TempData["StudentNumber"] = ViewBag.StudentNumber;
            TempData["PhoneNumber"] = ViewBag.PhoneNumber;
            TempData["Gender"] = ViewBag.Gender;


            return View();
        }

        // POST: HostelMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentNumber,StudentName,HostelName,Class,HostelFee,Gender,PhoneNumber,RegisteredBy,DateRecorded")] HostelMember hostelMember)
        {
            //getting the user
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            //Saving TempData into Database 
            hostelMember.StudentName = Convert.ToString(TempData["StudentName"]);
            hostelMember.StudentNumber = Convert.ToString(TempData["StudentNumber"]);
            hostelMember.Gender = Convert.ToString(TempData["Gender"]);
            hostelMember.PhoneNumber = Convert.ToString(TempData["PhoneNumber"]);

            hostelMember.DateRecorded = Convert.ToString(DateTime.Now.Date);
            hostelMember.RegisteredBy = user.FullName;

            //converting Route and Driver Value
            int ClassId = Convert.ToInt32(hostelMember.Class);
            int HostelId = Convert.ToInt32(hostelMember.HostelName);
            Hostel hostel = db.Hostels.Find(ClassId);
            Hostel hostelNAME = db.Hostels.Find(HostelId);

            hostelMember.Class = hostel.Class;
            hostelMember.HostelName = hostelNAME.HostelName;


            //Saving all Records to Database
            db.HostelMembers.Add(hostelMember);
            db.SaveChanges();


            ViewBag.HostelName = new SelectList(db.Hostels, "Id", "HostelName");
            ViewBag.Class = new SelectList(db.Hostels, "Id", "Class");

            return RedirectToAction("Index");


           
        }

        // GET: HostelMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HostelMember hostelMember = db.HostelMembers.Find(id);
            if (hostelMember == null)
            {
                return HttpNotFound();
            }
            ViewBag.HostelName = new SelectList(db.Hostels, "Id", "HostelName");
            ViewBag.Class = new SelectList(db.Hostels, "Id", "Class");
            return View(hostelMember);
        }

        // POST: HostelMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentNumber,StudentName,HostelName,Class,HostelFee,Gender,PhoneNumber,RegisteredBy,DateRecorded")] HostelMember hostelMember)
        {
            //getting the user
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            hostelMember.DateRecorded = Convert.ToString(DateTime.Now.Date);
            hostelMember.RegisteredBy = user.FullName;

            //converting Route and Driver Value
            int ClassId = Convert.ToInt32(hostelMember.Class);
            int HostelId = Convert.ToInt32(hostelMember.HostelName);
            Hostel hostel = db.Hostels.Find(ClassId);
            Hostel hostelNAME = db.Hostels.Find(HostelId);

            hostelMember.Class = hostel.Class;
            hostelMember.HostelName = hostelNAME.HostelName;

            if (ModelState.IsValid)
            {
                db.Entry(hostelMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HostelName = new SelectList(db.Hostels, "Id", "HostelName");
            ViewBag.Class = new SelectList(db.Hostels, "Id", "Class");
            return View(hostelMember);
        }

        // GET: HostelMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HostelMember hostelMember = db.HostelMembers.Find(id);
            if (hostelMember == null)
            {
                return HttpNotFound();
            }
            return View(hostelMember);
        }

        // POST: HostelMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HostelMember hostelMember = db.HostelMembers.Find(id);
            db.HostelMembers.Remove(hostelMember);
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
