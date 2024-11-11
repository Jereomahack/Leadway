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
    public class EmailingsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Emailings
        [Authorize(Roles = "Admin,Accountant,Principal,Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
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

            var info = from r in db.Emailings
                       select r;

            if (!String.IsNullOrEmpty(searchString))
            {
                info = info.Where(r => r.DateSent.Contains(searchString)
                                       || r.Statues.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    info = info.OrderByDescending(r => r.DateSent);
                    break;
                //    //case "Date":
                //    //    students = students.OrderBy(s => s.EnrollmentDate);
                //    //    break;
                //    //case "date_desc":
                //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    //    break;
                default:  // Name ascending 
                    info = info.OrderBy(r => r.DateSent);
                    break;
            }

            //int pageSize = 20;
            //int pageNumber = (page ?? 1);

            //getting user
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            if (searchString != null)
            {
                return View(db.Emailings.ToList().Where(t => t.Statues == searchString || t.DateSent == searchString));

            }
            else
            {
                if (User.IsInRole("Admin"))
                {
                    return View(db.Emailings.ToList().Where(t => t.Reciever == "Admin"));
                }
                return View(db.Emailings.ToList().Where(t =>t.Reciever == user.Name));
            }
        }



        // GET: Emailings/Details/5
        [Authorize(Roles = "Admin,Accountant,Principal,Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emailing emailing = db.Emailings.Find(id);
            if (emailing == null)
            {
                return HttpNotFound();
            }

            //Setting Statues to Read
            emailing.Statues = "Read";
            db.Entry(emailing).State = EntityState.Modified;
            db.SaveChanges();

            return View(emailing);
        }

        // GET: Emailings/Create
        [Authorize(Roles = "Admin,Accountant,Principal")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emailings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Accountant,Principal")]
        public ActionResult Create([Bind(Include = "Id,Sender,Statues,DateSent,Message,Reciever,Term,Subject")] Emailing emailing)
        {
            //Getting Sender
            if (User.IsInRole("Admin"))
            {
                emailing.Sender = "Admin";
            }
            else
            {
                Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
                emailing.Sender = user.Name;
            }

            //Saving Date Sent
            emailing.DateSent = Convert.ToString(DateTime.Now);

            //saving Term
            GlobalSettings term = db.GlobalSettings.FirstOrDefault(t => t.Name == "Term");
            emailing.Term = term.Value;

            //saving Statues
            emailing.Statues = "Not Read";


            if (ModelState.IsValid)
            {
                db.Emailings.Add(emailing);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emailing);
        }



        // GET: Emailings/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emailing emailing = db.Emailings.Find(id);
            if (emailing == null)
            {
                return HttpNotFound();
            }
            return View(emailing);
        }

        // POST: Emailings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Emailing emailing = db.Emailings.Find(id);
            db.Emailings.Remove(emailing);
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
