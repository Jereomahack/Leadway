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
    public class StaffPayrollsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StaffPayrolls
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6,Principal,Accountant,Cashier")]
        public ActionResult Index()
        {
            //getting user
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            return View(db.StaffPayrolls.ToList().Where(t => t.StaffNumber == user.StaffNumber));
        }

        // GET: StaffPayrolls
        [Authorize(Roles = "Admin,Accountant,Principal")]
        public ActionResult IndexAll()
        {

            return View(db.StaffPayrolls.ToList());
        }

        // GET: StaffPayrolls/Details/5
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6,Principal,Accountant,Admin")]
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffPayroll staffPayroll = db.StaffPayrolls.Find(id);
            if (staffPayroll == null)
            {
                return HttpNotFound();
            }

            ViewBag.Month = staffPayroll.Month;
            ViewBag.Session = staffPayroll.Session;
            return View(staffPayroll);
        }

        // GET: StaffPayrolls/Create
        [Authorize(Roles = "Accountant")]
        public ActionResult Create(int id)
        {
            //finding Staff by Id
            Teachers staff = db.Teachers.Find(id);

            //getting staff values into ViewBAG
            ViewBag.StaffName = staff.FullName;
            ViewBag.StaffNumber = staff.StaffNumber;
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.salary = staff.BasicSalary / 12;

            return View();
        }

        // POST: StaffPayrolls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant")]
        public ActionResult Create([Bind(Include = "Id,StaffNumber,StaffName,Session,Term,Month,Amount,Balance,EnteredBy,DateofPayment")] StaffPayroll staffPayroll, decimal Salary, string Name, string Number)
        {

            //Getting User
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            //Searching Stafff from DB
            Teachers teacher = db.Teachers.FirstOrDefault(t => t.StaffNumber == staffPayroll.StaffNumber);

            //saving session
            int sessionId = Convert.ToInt32(staffPayroll.Session);
            Session session = db.Sessions.Find(sessionId);
            staffPayroll.Session = session.AcademicYear;

            //Searching transaction table if exist
            //Transactions transact = db.Transactionss.FirstOrDefault(t => t.Number == staffPayroll.StaffNumber && t.Month == staffPayroll.Month && t.year == staffPayroll.Session && t.Terms == staffPayroll.Term);
            StaffPayroll salaryExist = db.StaffPayrolls.FirstOrDefault(t => t.StaffNumber == Number && t.Month == staffPayroll.Month && t.Session == staffPayroll.Session && t.Term == staffPayroll.Term);

            if (salaryExist == null)
            {

                staffPayroll.Amount = Salary;

                //saving Name and Number
                staffPayroll.StaffName = Name;
                staffPayroll.StaffNumber = Number;


                //checking if Staff Collected  Loan
                Loan Sloan = db.Loans.FirstOrDefault(s => s.StaffNumber == staffPayroll.StaffNumber&&s.AllowDeduction=="Yes");
                if (Sloan != null)
                {
                    if (Sloan.AmountPaid == Sloan.AmountCollected)
                    {
                        staffPayroll.AmountPaidAfterDeduction = Salary;
                        Sloan.AmountLeft = 0;
                        Sloan.AmountPaid = Sloan.AmountPaid;
                        staffPayroll.AmountDeducted = 0;
                    }

                    else if (Sloan.AmountLeft >= Sloan.AmountPaid)
                    {
                        staffPayroll.AmountPaidAfterDeduction = Salary - Sloan.AmountPayable;
                        Sloan.AmountLeft -= Sloan.AmountPayable;
                        Sloan.AmountPaid += Sloan.AmountPayable;
                        staffPayroll.AmountDeducted = Sloan.AmountPayable;
                    }

                    else if (Sloan.AmountLeft <= Sloan.AmountPaid)
                    {

                        if (Sloan.AmountLeft == Sloan.AmountPayable)
                        {
                            staffPayroll.AmountPaidAfterDeduction = Salary - Sloan.AmountPayable;
                            Sloan.AmountLeft -= Sloan.AmountPayable;
                            Sloan.AmountPaid += Sloan.AmountPayable;
                            staffPayroll.AmountDeducted = Sloan.AmountPayable;
                        }
                        else if (Sloan.AmountLeft < Sloan.AmountPayable)
                        {
                            staffPayroll.AmountPaidAfterDeduction = Salary - Sloan.AmountLeft;
                            Sloan.AmountPaid += Sloan.AmountLeft;
                            Sloan.AmountLeft = 0;
                            staffPayroll.AmountDeducted = Sloan.AmountLeft;
                        }

                        else
                        {
                            staffPayroll.AmountPaidAfterDeduction = Salary - Sloan.AmountPayable;
                            Sloan.AmountLeft -= Sloan.AmountPayable;
                            Sloan.AmountPaid += Sloan.AmountPayable;
                            staffPayroll.AmountDeducted = Sloan.AmountPayable;
                        }

                    }

                }
                else
                {
                    staffPayroll.AmountPaidAfterDeduction = Salary;
                    staffPayroll.AmountDeducted = 0;
                }

                //updating Loan
                db.Entry(Sloan).State = EntityState.Modified;
                db.SaveChanges();

                //bonus amount
                staffPayroll.BonusAmount = 0;

                //saving other records
                staffPayroll.EnteredBy = user.Name;
                staffPayroll.DateofPayment = DateTime.Now;

                //Saving all Records to Db
                db.StaffPayrolls.Add(staffPayroll);
                db.SaveChanges();

                //Searching for Bonus
                Bonus bonusexist = db.Bonus.FirstOrDefault(d => d.StaffNumber == staffPayroll.StaffNumber && d.Term == staffPayroll.Term && d.Month == staffPayroll.Month && d.Session == staffPayroll.Session);
                if (bonusexist != null)
                {
                    var x = db.Bonus.Where(d => d.StaffNumber == staffPayroll.StaffNumber && d.Term == staffPayroll.Term && d.Month == staffPayroll.Month && d.Session == staffPayroll.Session&&d.AllowDeduction=="Yes").Sum(s => s.BonusAmount);
                    //updating staff salary
                    StaffPayroll stafSalary = db.StaffPayrolls.FirstOrDefault(s => s.StaffNumber == staffPayroll.StaffNumber && s.Term == staffPayroll.Term && s.Month == staffPayroll.Month && s.Session == staffPayroll.Session);
                    stafSalary.AmountPaidAfterDeduction += x;
                    stafSalary.BonusAmount = x;


                    //updating Loan
                    db.Entry(stafSalary).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    staffPayroll.AmountPaidAfterDeduction += 0;
                    staffPayroll.BonusAmount += 0;
                }

                //Searching for deduction
                Deduction Deductionexist = db.Deductions.FirstOrDefault(d => d.StaffNumber == staffPayroll.StaffNumber && d.Term == staffPayroll.Term && d.Month == staffPayroll.Month && d.Session == staffPayroll.Session);
                if (Deductionexist != null)
                {
                    var y = db.Deductions.Where(d => d.StaffNumber == staffPayroll.StaffNumber && d.Term == staffPayroll.Term && d.Month == staffPayroll.Month && d.Session == staffPayroll.Session&&d.AllowDeduction=="Yes").Sum(s => s.DeductionAmount);
                    //updating staff salary
                    StaffPayroll stafSalary = db.StaffPayrolls.FirstOrDefault(s => s.StaffNumber == staffPayroll.StaffNumber && s.Term == staffPayroll.Term && s.Month == staffPayroll.Month && s.Session == staffPayroll.Session);
                    stafSalary.AmountPaidAfterDeduction -= y;
                    stafSalary.AmountDeducted += y;


                    //updating Loan
                    db.Entry(stafSalary).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    staffPayroll.AmountPaidAfterDeduction += 0;
                    staffPayroll.AmountDeducted += 0;
                }


            }
            else
            {
                ViewBag.NoDetail = "please Ensure Salary of this Staff for this month and Academic Sesssion has not been Entered before";

                ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");


                ViewBag.StaffName = Name;
                ViewBag.StaffNumber = Number;
                return View();
            }

            int id = staffPayroll.Id;

            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            return RedirectToAction("Details", new { id });
        }

        // GET: StaffPayrolls/Edit/5
        [Authorize(Roles = "Accountant")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StaffPayroll staffPayroll = db.StaffPayrolls.Find(id);
            if (staffPayroll == null)
            {
                return HttpNotFound();
            }
            //getting StaffPayroll Session
            StaffPayroll payroll = db.StaffPayrolls.Find(id);
            ViewBag.session = payroll.Session;

            //saving viewbag values into TempData
            TempData["session"] = ViewBag.session;



            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            return View(staffPayroll);
        }

        // POST: StaffPayrolls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant")]
        public ActionResult Edit([Bind(Include = "Id,StaffNumber,StaffName,Session,Term,Month,Amount,Balance,EnteredBy,DateofPayment")] StaffPayroll staffPayroll)
        {

            //Getting User
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            //saving other records
            staffPayroll.EnteredBy = user.Name;
            staffPayroll.DateofPayment = DateTime.Now;

            //saving session
            staffPayroll.Session = Convert.ToString(TempData["session"]);

            //getting value from transaction table
            Transactions amtinDB = db.Transactionss.FirstOrDefault(a => a.Number == staffPayroll.StaffNumber && a.Terms == staffPayroll.Term && a.Month == staffPayroll.Month && a.year == staffPayroll.Session);
            var Amt = amtinDB.AmountPaid;
            var staffPAmt = staffPayroll.Amount;

            //updating values of the Staff Salary
            staffPayroll.Amount = Amt + staffPAmt;

            if (ModelState.IsValid)
            {
                db.Entry(staffPayroll).State = EntityState.Modified;
                db.SaveChanges();


                //saving into Transaction Table
                Transactions transaction1 = new Transactions()
                {
                    Number = staffPayroll.StaffNumber,
                    Name = staffPayroll.StaffName,
                    AmountPaid = staffPayroll.Amount,
                    year = staffPayroll.Session,
                    Terms = staffPayroll.Term,
                    Month = staffPayroll.Month,
                    DatePaid = Convert.ToString(staffPayroll.DateofPayment),
                };
                db.Transactionss.Add(transaction1);
                db.SaveChanges();

                return RedirectToAction("IndexAll");
            }
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            return View(staffPayroll);
        }

        // GET: StaffPayrolls/Delete/5
        //[Authorize(Roles = "Accountant")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    StaffPayroll staffPayroll = db.StaffPayrolls.Find(id);
        //    if (staffPayroll == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(staffPayroll);
        //}

        // POST: StaffPayrolls/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Accountant")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    StaffPayroll staffPayroll = db.StaffPayrolls.Find(id);
        //    db.StaffPayrolls.Remove(staffPayroll);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
