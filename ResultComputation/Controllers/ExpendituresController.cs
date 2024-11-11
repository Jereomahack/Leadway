using LightWay.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class ExpendituresController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Expenditures
        [Authorize(Roles = "Principal,Accountant")]
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

            var Expenditure = from r in db.Expenditures
                              select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                Expenditure = Expenditure.Where(r => r.Vissibility.Contains(searchString)
                                        || r.Statues.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    Expenditure = Expenditure.OrderByDescending(r => r.DateRecorded);
                    break;
                //    //case "Date":
                //    //    students = students.OrderBy(s => s.EnrollmentDate);
                //    //    break;
                //    //case "date_desc":
                //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    //    break;
                default:  // Name ascending 
                    Expenditure = Expenditure.OrderBy(r => r.DateRecorded);
                    break;
            }

            //int pageSize = 20;
            //int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                return View(db.Expenditures.ToList().Where(t => t.Statues == searchString || t.Vissibility == searchString));

            }
            else
            {
                if (User.IsInRole("Accountant"))
                {
                    return View(db.Expenditures.ToList().Where(t => t.Statues == "Decline" || t.Statues == "Approved"));
                }
                else if (User.IsInRole("Principal"))
                {
                    return View(db.Expenditures.ToList().Where(t => t.Statues == "Not Approve" || t.Statues == "Approved" && t.Vissibility == "Principal"));
                }
                return View(db.Expenditures.ToList().Where(t => t.Statues == "Approved"));
            }
        }

        // GET: Expenditures/Details/5
        [Authorize(Roles = "Principal,Accountant")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expenditure expenditure = db.Expenditures.Find(id);
            if (expenditure == null)
            {
                return HttpNotFound();
            }
            return View(expenditure);
        }


        public ActionResult IndexpExp(string ExpType, string SearchFrom, string SearchTo)
        {
            ViewBag.ExpType = new SelectList(db.exptypes.OrderBy(t => t.ExpensesName), "ExpensesName", "ExpensesName");
            if (ExpType != null && SearchFrom != null && SearchTo != null)
            {
                if (ExpType != "" && SearchFrom != "" && SearchTo != "")
                {
                    var datefrom = Convert.ToDateTime(SearchFrom);
                    var dateto = Convert.ToDateTime(SearchTo);
                    return View(db.Expenditures.Where(t => t.ExpenditureType == ExpType && t.DateRecorded >= datefrom && t.DateRecorded <= dateto).ToList());
                }
                if (ExpType == "" && SearchFrom != "" && SearchTo != "")
                {
                    var datefrom = Convert.ToDateTime(SearchFrom);
                    var dateto = Convert.ToDateTime(SearchTo);
                    return View(db.Expenditures.Where(t => t.DateRecorded >= datefrom && t.DateRecorded <= dateto).ToList());
                }
            }


            return View(db.Expenditures.OrderByDescending(t => t.Id).Take(20).ToList());
        }

        // GET: Expenditures/Create
        [Authorize(Roles = "Accountant")]
        public ActionResult Create()
        {
            ViewBag.ExpenditureType = new SelectList(db.exptypes.OrderBy(t => t.ExpensesName), "ExpensesName", "ExpensesName");
            return PartialView();
        }

        // POST: Expenditures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant")]
        public ActionResult Create([Bind(Include = "Id,Amount,AmountRequested,AmountReleased,DateRequested,Purpose,Vissibility,RequestedBy,DeclineReason,DateRecorded,ExpenditureType,Statues")] Expenditure expenditure)
        {
            //Coverting Date to STRING
            expenditure.DateRecorded = DateTime.Now.Date;

            //getting Term 
            GlobalSettings term = db.GlobalSettings.FirstOrDefault(t => t.Name == "Term");
            expenditure.Term = term.Value;

            //getting Session
            GlobalSettings session = db.GlobalSettings.FirstOrDefault(s => s.Name == "Session");
            expenditure.Session = session.Value;

            //saving statues
            expenditure.Statues = "Approved";
            expenditure.ApprovedBy = User.Identity.Name;

            //Saving Records To Database
            db.Expenditures.Add(expenditure);
            db.SaveChanges();

            string message = expenditure.ExpenditureType + " was successfully entered. Thank You.";
            return Json(new { message });



        }

        // GET: Expenditures/Edit/5
        [Authorize(Roles = "Accountant,Principal")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expenditure expenditure = db.Expenditures.Find(id);
            if (expenditure == null)
            {
                return HttpNotFound();
            }
            return View(expenditure);
        }

        // POST: Expenditures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Accountant,Principal")]
        public ActionResult Edit([Bind(Include = "Id,Amount,AmountRequested,AmountReleased,ExpenditureType,DateRequested,Purpose,Vissibility,RequestedBy,DeclineReason,DateRecorded,Statues")] Expenditure expenditure)
        {

            //getting Term 
            GlobalSettings term = db.GlobalSettings.FirstOrDefault(t => t.Name == "Term");
            expenditure.Term = term.Value;

            //getting Session
            GlobalSettings session = db.GlobalSettings.FirstOrDefault(s => s.Name == "Session");
            expenditure.Session = session.Value;
            if (User.IsInRole("Accountant"))
            {
                //getting User
                Teachers user1 = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
                //Saving other Details
                expenditure.Statues = "Not Approve";
                expenditure.RequestedBy = user1.Name;
            }

            else if (User.IsInRole("principal"))
            {
                //getting User
                Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
                //Saving other Details
                expenditure.Statues = "Decline";
                expenditure.RequestedBy = user.Name;
            }


            if (ModelState.IsValid)
            {
                db.Entry(expenditure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expenditure);
        }

        //Approval
        public ActionResult Approve(int id)
        {

            //Finding User
            Teachers user = db.Teachers.FirstOrDefault(t => t.EmailAddress == User.Identity.Name);

            //Finding Expenditure
            Expenditure expenditure = db.Expenditures.Find(id);
            expenditure.Statues = "Approved";
            expenditure.DateRecorded = DateTime.Now.Date;
            expenditure.ApprovedBy = user.Name;

            db.Entry(expenditure).State = EntityState.Modified;
            db.SaveChanges();


            int Id = expenditure.Id;
            return RedirectToAction("Index");
        }

        // GET: Expenditures/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Expenditure expenditure = db.Expenditures.Find(id);
        //    if (expenditure == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(expenditure);
        //}

        // POST: Expenditures/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Expenditure expenditure = db.Expenditures.Find(id);
        //    db.Expenditures.Remove(expenditure);
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
