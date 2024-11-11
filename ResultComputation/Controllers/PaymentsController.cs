using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LightWay.Models;
using PagedList;

namespace LightWay.Controllers
{
    public class PaymentsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Payments
        [Authorize(Roles = "Accountant,Principal,Cashier")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;

            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //var ivadostaff = db.IvadoStaff.Include(i => i.Branch).Include(i => i.Position);

            var payment = from r in db.payments
                          select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                payment = payment.Where(r => r.StudentName.Contains(searchString)
                                       || r.Class.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    payment = payment.OrderByDescending(r => r.DateRecorded);
                    break;
                //    //case "Date":
                //    //    students = students.OrderBy(s => s.EnrollmentDate);
                //    //    break;
                //    //case "date_desc":
                //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    //    break;
                default:  // Name ascending 
                    payment = payment.OrderBy(r => r.DateRecorded);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                return View(db.payments.OrderByDescending(i => i.StudentName == searchString || i.Class == searchString).ToPagedList(pageNumber, pageSize));

            }
            else
            {
                return View(db.payments.OrderBy(i => i.DateRecorded).ToPagedList(pageNumber, pageSize));

            }
        }

        // GET: Payments/Details/5
        [Authorize(Roles = "Accountant,Principal,Cashier")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        [Authorize(Roles = "Accountant")]
        public ActionResult Create(int id)
        {
            //getting student by Id
            Student student = db.Students.Find(id);
            //getting student full Name
            var surname = student.Surname;
            var othername = student.OtherName;
            ViewBag.StudentName = surname + " " + othername;
            //getting class and Term of the student
            ViewBag.StudentNumber = student.StudentNumber;
            ViewBag.Class = student.Class;

            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.PaymentType = new SelectList(db.PaymentTypes, "Id", "PaymentTypeName");

            //Saving viewbag values into TempData
            TempData["StudentNumber"] = ViewBag.StudentNumber;
            TempData["StudentName"] = ViewBag.StudentName;
            TempData["Class"] = ViewBag.Class;

            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Create([Bind(Include = "Id,StudentName,Class,Term,Amount,Balance,EnteredBy,DateRecorded,Session,PaymentFor,AmountPaid,StudentNum")] Payment payment, string Session, string PaymentFor)
        {
            
            //getting Session 
            int SessionId = Convert.ToInt32(Session);
            Session sessssion = db.Sessions.FirstOrDefault(s => s.Id == SessionId);
            payment.Session = sessssion.AcademicYear;

            //getting PaymenTtYPE
            int PaymentId = Convert.ToInt32(PaymentFor);
            PaymentType paymentt = db.PaymentTypes.FirstOrDefault(p => p.Id == PaymentId);
            payment.PaymentFor = paymentt.PaymentTypeName;

            //Getting User Name
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            payment.EnteredBy = user.FullName;
            payment.DateRecorded = DateTime.Now;

            //saving other records
            payment.StudentName = Convert.ToString(TempData["StudentName"]);
            payment.StudentNum = Convert.ToString(TempData["StudentNumber"]);
            payment.Class = Convert.ToString(TempData["Class"]);

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                db.payments.Add(payment);
                db.SaveChanges();

                //saving into Transaction Table
                Transactions transaction = new Transactions()
                {
                    Number = payment.StudentNum,
                    Name = payment.StudentName,
                    AmountPaid = payment.Amount,
                    year = payment.Session,
                    Terms = payment.Term,
                    DatePaid = Convert.ToString(payment.DateRecorded),

                };
                db.Transactionss.Add(transaction);
                db.SaveChanges();


            }
            ViewBag.Class = new SelectList(db.ClassLists, "Id", "ClassName");
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.PaymentType = new SelectList(db.PaymentTypes, "Id", "PaymentTypeName");
            return RedirectToAction("Index");
        }

        // GET: Payments/Edit/5
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }

            ViewBag.Class = new SelectList(db.ClassLists, "Id", "ClassName");
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.PaymentType = new SelectList(db.PaymentTypes, "Id", "PaymentTypeName");
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant,Cashier")]
        public ActionResult Edit([Bind(Include = "Id,StudentName,Class,Term,Amount,Balance,PaymentFor,EnteredBy,DateRecorded,Session,StudentNum")] Payment payment, string Class, string Session, string PaymentFor)
        {
           
            //Getting User Name
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            payment.EnteredBy = user.FullName;
            payment.DateRecorded = DateTime.Now;

            payment.EnteredBy = User.Identity.Name;

            //getting Class Name
            int ClassId = Convert.ToInt32(Class);
            ClassList Classs = db.ClassLists.Find(ClassId);
            payment.Class = Classs.ClassName;

            //getting Session 
            int SessionId = Convert.ToInt32(Session);
            Session sessssion = db.Sessions.Find(SessionId);
            payment.Session = sessssion.AcademicYear;

            //getting PaymenTtYPE
            int PaymentId = Convert.ToInt32(PaymentFor);
            PaymentType paymentt = db.PaymentTypes.Find( PaymentId);
            payment.PaymentFor = paymentt.PaymentTypeName;

            //getting Amount Paid from Transaction Table
            Transactions transaction = db.Transactionss.FirstOrDefault(t => t.Name == payment.StudentName && t.year == payment.Session && t.Terms == payment.Term);
            decimal amtindb = transaction.AmountPaid;
            decimal Tamount = amtindb + payment.Amount;
            payment.Amount = Tamount;
            



            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();

                //saving into Transaction Table
                Transactions transaction1 = new Transactions()
                {
                    Number = payment.StudentNum,
                    Name = payment.StudentName,
                    AmountPaid = payment.Amount,
                    year = payment.Session,
                    Terms = payment.Term,
                    DatePaid = Convert.ToString(payment.DateRecorded),
                };
                db.Transactionss.Add(transaction1);
                db.SaveChanges();


                ViewBag.Class = new SelectList(db.ClassLists, "Id", "ClassName");
                ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
                ViewBag.PaymentType = new SelectList(db.PaymentTypes, "Id", "PaymentTypeName");

                return RedirectToAction("Index");
            }
            ViewBag.Class = new SelectList(db.ClassLists, "Id", "ClassName");
            ViewBag.Session = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.PaymentType = new SelectList(db.PaymentTypes, "Id", "PaymentTypeName");
            return View(payment);
        }

        // GET: Payments/Delete/5
        //[Authorize(Roles = "Accountant")]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Payment payment = db.payments.Find(id);
        //    if (payment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(payment);
        //}

        // POST: Payments/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Payment payment = db.payments.Find(id);
        //    db.payments.Remove(payment);
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
