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
    public class LoansController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Loans
        [Authorize(Roles = "Accountant,Principal")]
        public ActionResult Index()
        {
            return View(db.Loans.ToList());
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create(int id)
        {
            Teachers teacher = db.Teachers.Find(id);
            ViewBag.Name = teacher.FullName;
            ViewBag.Number = teacher.StaffNumber;

            ViewBag.Staff = new SelectList(db.Teachers, "Id", "FullName");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StaffNumber,StaffName,AmountCollected,AmountLeft,AmountPayable,AmountPaid,CreatedBy,DateRecorded,Reason,Statues")] Loan loan, string Number, string Name)
        {

            //Setting Statues to Closed
            loan.Statues = "Closed";

            //setting Permission for Deducting Loan
            loan.AllowDeduction = "Yes";

            //getting Staff Name
            loan.StaffName = Name;

            //getting staff Number
            loan.StaffNumber = Number;

            //amount left
            loan.AmountLeft = loan.AmountCollected;

            //amountpaid
            loan.AmountPaid = 0;

            //createdby
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            loan.CreatedBy = user.Name;

            //date Recorded
            loan.DateRecorded = DateTime.Now;

            //searching if Loan exist
            Loan loanexist = db.Loans.FirstOrDefault(l => l.StaffNumber == loan.StaffNumber);
            if (loanexist != null)
            {
                loanexist.AmountCollected += loan.AmountCollected;
                loanexist.AmountLeft += loan.AmountLeft;

                db.Entry(loanexist).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                //Saving Records to DATABASE
                db.Loans.Add(loan);
                db.SaveChanges();
            }

            //saving loan into LoanTransaction 
            LoanTransact loantransact = new LoanTransact()
            {
                StaffNumber = loan.StaffNumber,
                StaffName = loan.StaffName,
                AmountCollected = loan.AmountCollected,
                Payable = loan.AmountPayable,
                Reason = loan.Reason,
                CreatedBy = loan.CreatedBy,
                DateRecorded = loan.DateRecorded,

            };
            //Saving Records to DATABASE
            db.LoanTransacts.Add(loantransact);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Name = loan.StaffName;
            ViewBag.Number = loan.StaffNumber;

            ViewBag.Staff = new SelectList(db.Teachers, "Id", "FullName");
            return View(loan);
        }

        //// POST: Loans/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StaffNumber,StaffName,AmountCollected,AmountLeft,AmountPayable,AmountPaid,CreatedBy,DateRecorded,Statues,Reason,")] Loan loan, string Number, string Name)
        {
            //Setting Statues to Closed
            loan.Statues = "Closed";



            //getting Staff Name
            loan.StaffName = Name;

            //getting staff Number
            loan.StaffNumber = Number;

            //createdby
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            loan.CreatedBy = user.Name;

            //date Recorded
            loan.DateRecorded = DateTime.Now;
            
            //Updating Records
            db.Entry(loan).State = EntityState.Modified;
            db.SaveChanges();


            //saving loan into LoanTransaction 
            LoanTransact loantransact = new LoanTransact()
            {
                StaffNumber = loan.StaffNumber,
                StaffName = loan.StaffName,
                AmountCollected = loan.AmountCollected,
                Payable = loan.AmountPayable,
                Reason = loan.Reason,
                CreatedBy = loan.CreatedBy,
                DateRecorded = loan.DateRecorded,

            };
            //Saving Records to DATABASE
            db.LoanTransacts.Add(loantransact);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        //// GET: Loans/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Loan loan = db.Loans.Find(id);
        //    if (loan == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(loan);
        //}

        //// POST: Loans/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Loan loan = db.Loans.Find(id);
        //    db.Loans.Remove(loan);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //Allowing Loan Edit
        public ActionResult Allow(int? id)
        {
            Loan loan = db.Loans.Find(id);
            loan.Statues = "Open";

            //Updating Records
            db.Entry(loan).State = EntityState.Modified;
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
