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
    public class BonusesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bonuses
        [Authorize(Roles = "Accountant,Principal,Cashier")]
        public ActionResult Index()
        {
            return View(db.Bonus.ToList());
        }

        // GET: Bonuses/Details/5
        [Authorize(Roles = "Accountant,Cashier,Principal")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bonus bonus = db.Bonus.Find(id);
            if (bonus == null)
            {
                return HttpNotFound();
            }
            return View(bonus);
        }

        // GET: Bonuses/Create
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Create(int id)
        {
            //getting Staff Details
            Teachers teacher = db.Teachers.Find(id);
            ViewBag.Name = teacher.FullName;
            ViewBag.Number = teacher.StaffNumber;
            ViewBag.BonusType = new SelectList(db.BonusTypes, "Id", "BonusTypeName");
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            return View();
        }

        // POST: Bonuses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Create([Bind(Include = "Id,StaffNumber,StaffName,BonusType,BonusAmount,Session,Term,Month,EnteredBy,DateCreated,Statues")] Bonus bonus, string BonusType, string Name, string Number, string Session)
        {

            //setting Statues to Closed
            bonus.Statues = "Closed";

            //saving Records
            bonus.StaffNumber = Number;
            bonus.StaffName = Name;

            //saving Deduction Type
            int typeId = Convert.ToInt32(BonusType);
            BonusType dtype = db.BonusTypes.Find(typeId);
            bonus.BonusType = dtype.BonusTypeName;

            //getting Sesssion
            int SessionId = Convert.ToInt32(Session);
            Session Sess = db.Sessions.Find(SessionId);
            bonus.Session = Sess.AcademicYear;

            bonus.DateCreated = DateTime.Now;

            //getting User
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            bonus.EnteredBy = user.Name;

            //Saving int Database
            db.Bonus.Add(bonus);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Bonuses/Edit/5
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bonus bonus = db.Bonus.Find(id);
            if (bonus == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = new SelectList(db.BonusTypes, "Id", "BonusTypeName");
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            return View(bonus);
        }

        // POST: Bonuses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Edit([Bind(Include = "Id,StaffNumber,StaffName,BonusType,BonusAmount,Session,Term,Month,EnteredBy,DateCreated,Statues")] Bonus bonus, string Type, string Session)
        {

            //setting Statues to Closed
            bonus.Statues = "Closed";

            if(bonus.Session==null && bonus.BonusType==null)
            {
                //saving Deduction Type
                int typeId = Convert.ToInt32(Type);
                BonusType dtype = db.BonusTypes.Find(typeId);
                bonus.BonusType = dtype.BonusTypeName;

                //getting Sesssion
                int SessionId = Convert.ToInt32(Session);
                Session Sess = db.Sessions.Find(SessionId);
                bonus.Session = Sess.AcademicYear;
            }
          


            //getting User
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            bonus.EnteredBy = user.Name;

            bonus.DateCreated = DateTime.Now;

            //Updating Records
            db.Entry(bonus).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        // GET: Bonuses/Delete/5
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bonus bonus = db.Bonus.Find(id);
            if (bonus == null)
            {
                return HttpNotFound();
            }
            return View(bonus);
        }

        // POST: Bonuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult DeleteConfirmed(int id)
        {
            Bonus bonus = db.Bonus.Find(id);
            db.Bonus.Remove(bonus);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //principal Allow Edit Action
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Allow(int id)
        {
            Bonus bonus = db.Bonus.Find(id);
            bonus.Statues = "Open";

            db.Entry(bonus).State = EntityState.Modified;
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
