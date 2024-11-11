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
    public class SchoolSubjectsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SchoolSubjects
        [Authorize(Roles ="Admin,Teacher,Form Master,Accountant,Principal")]
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

            var subject = from r in db.SchoolSubjectss
                          select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                subject = subject.Where(r => r.Subject.Contains(searchString)
                                      );
            }
            switch (sortOrder)
            {
                case "name_desc":
                    subject = subject.OrderByDescending(r => r.Subject);
                    break;
                //    //case "Date":
                //    //    students = students.OrderBy(s => s.EnrollmentDate);
                //    //    break;
                //    //case "date_desc":
                //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    //    break;
                default:  // Name ascending 
                    subject = subject.OrderBy(r => r.Subject);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                //return View(db.Teachers.ToList().Where(c => c.StaffNumber == searchString));
                return View(db.SchoolSubjectss.OrderByDescending(i => i.Subject == searchString).ToPagedList(pageNumber, pageSize));
            }
            else
            {

                return View(db.SchoolSubjectss.OrderBy(i => i.Subject).ToPagedList(pageNumber, pageSize));
                //return View(db.Students.ToList());
            }
            
           
        }

       

        // GET: SchoolSubjects/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolSubjects schoolSubjects = db.SchoolSubjectss.Find(id);
            if (schoolSubjects == null)
            {
                return HttpNotFound();
            }
            return View(schoolSubjects);
        }

        // POST: SchoolSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            SchoolSubjects schoolSubjects = db.SchoolSubjectss.Find(id);
            db.SchoolSubjectss.Remove(schoolSubjects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        // GET: SchoolSubjects/Create/5
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Subject")] SchoolSubjects schoolSubjects)
        {

            db.SchoolSubjectss.Add(schoolSubjects);
            db.SaveChanges();

            int id = schoolSubjects.Id;
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
