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
    public class DeductionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Deductions
        [Authorize(Roles ="Accountant,Principal,Cashier")]
        public ActionResult Index()
        {
            return View(db.Deductions.ToList());
        }

        // GET: Deductions/Details/5

        [Authorize(Roles = "Accountant,Principal,Cashier")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deduction deduction = db.Deductions.Find(id);
            if (deduction == null)
            {
                return HttpNotFound();
            }
            return View(deduction);
        }

        // GET: Deductions/Create
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Create(int? id)
        {
            //getting Staff Name and Number
            Teachers teacher = db.Teachers.Find(id);

            ViewBag.Name = teacher.FullName;
            ViewBag.Number = teacher.StaffNumber;
            ViewBag.Type = new SelectList(db.DeductionTypes, "Id", "DeductionTypeName");
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            return View();
        }

        // POST: Deductions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Create([Bind(Include = "Id,StaffNumber,StaffName,DeductionType,DeductionAmount,Session,Term,Month,EnteredBy,DateofPayment,Statues")] Deduction deduction, string Type, string Name, string Number, string Session)
        {


            //setting Statues to Closed
            deduction.Statues = "Closed";

            //setting permission for Deduction
            deduction.AllowDeduction = "Yes";

            //saving Records
            deduction.StaffNumber = Number;
            deduction.StaffName = Name;

            //saving Deduction Type
            int typeId = Convert.ToInt32(Type);
            DeductionType dtype = db.DeductionTypes.Find(typeId);
            deduction.DeductionType = dtype.DeductionTypeName;

            //getting Sesssion
            int SessionId = Convert.ToInt32(Session);
            Session Sess = db.Sessions.Find(SessionId);
            deduction.Session = Sess.AcademicYear;

            //getting user
            Teachers stsff = db.Teachers.FirstOrDefault(s => s.EmailAddress == User.Identity.Name);
            deduction.EnteredBy = stsff.FullName;

            deduction.DateofDdeduction = DateTime.Now;

            //saving Records to Db
            db.Deductions.Add(deduction);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        // GET: Deductions/Edit/5
        [Authorize(Roles = "Accountant")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deduction deduction = db.Deductions.Find(id);
            if (deduction == null)
            {
                return HttpNotFound();
            }

            //getting Staff Name and Number
            Teachers teacher = db.Teachers.Find(id);

            ViewBag.Name = teacher.FullName;
            ViewBag.Number = teacher.StaffNumber;
            ViewBag.Type = new SelectList(db.DeductionTypes, "Id", "DeductionTypeName");
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            return View(deduction);
        }

        // POST: Deductions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant")]
        public ActionResult Edit([Bind(Include = "Id,StaffNumber,StaffName,DeductionType,DeductionAmount,Session,Term,Month,EnteredBy,DateofPayment,Statues")] Deduction deduction, string Type, string Name, string Number, string Session)
        {

            //setting Statues to Closed
            deduction.Statues = "Closed";

            //saving Records
            deduction.StaffNumber = Number;
            deduction.StaffName = Name;

            //saving Deduction Type
            int typeId = Convert.ToInt32(Type);
            DeductionType dtype = db.DeductionTypes.Find(typeId);
            deduction.DeductionType = dtype.DeductionTypeName;

            //getting Sesssion
            int SessionId = Convert.ToInt32(Session);
            Session Sess = db.Sessions.Find(SessionId);
            deduction.Session = Sess.AcademicYear;


            //Updating Records to Database
            db.Entry(deduction).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        // GET: Deductions/Delete/5
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deduction deduction = db.Deductions.Find(id);
            if (deduction == null)
            {
                return HttpNotFound();
            }
            return View(deduction);
        }

        // POST: Deductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult DeleteConfirmed(int id)
        {
            Deduction deduction = db.Deductions.Find(id);
            db.Deductions.Remove(deduction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Allowing Editing for Deduction
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Allow(int id)
        {
            Deduction deduction = db.Deductions.Find(id);
            deduction.Statues = "Open";

            //Making Deduction Editable
            db.Entry(deduction).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Allowing Removal of Deduction
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult AllowDeduction(int id)
        {
            Deduction deduction = db.Deductions.Find(id);
            if(deduction.AllowDeduction=="Yes")
            {
                deduction.AllowDeduction = "No";
            }
            else
            {
                deduction.AllowDeduction = "Yes";
            }
           
            db.Entry(deduction).State = EntityState.Modified;
            db.SaveChanges();
            return View();
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
