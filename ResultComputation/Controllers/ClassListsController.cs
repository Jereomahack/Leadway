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
    public class ClassListsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClassLists/Teacher
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Index()
        {
            //Teachers user = db.Teachers.FirstOrDefault(u=>u.EmailAddress==User.Identity.Name);
            //TeacherSubject Tclass = db.TeacherSubjects.FirstOrDefault(c=>c.TeacherNum==user.StaffNumber);
            return View(db.ClassLists.ToList());
        }

        [Authorize(Roles = "Admin,Accountant,Principal")]
        public ViewResult IndexAdmin(string sortOrder, string currentFilter, string searchString, int? page)
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

            var Class = from r in db.ClassLists
                        select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                Class = Class.Where(r => r.ClassName.Contains(searchString));

            }
            switch (sortOrder)
            {
                case "name_desc":
                    Class = Class.OrderByDescending(r => r.ClassName);
                    break;
                //    //case "Date":
                //    //    students = students.OrderBy(s => s.EnrollmentDate);
                //    //    break;
                //    //case "date_desc":
                //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    //    break;
                default:  // Name ascending 
                    Class = Class.OrderBy(r => r.ClassName);
                    break;
            }

            //int pageSize = 3;
            //int pageNumber = (page ?? 1);
            //return View(Class.ToPagedList(pageNumber, pageSize));

            return View(db.ClassLists.ToList());
        }

        // GET: ClassLists/Details/5
        //[Authorize]
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ClassList classList = db.ClassLists.Find(id);
        //    if (classList == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(classList);
        //}

        // GET: ClassLists/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.TeacherFiter = new SelectList(db.Teachers, "Id", "FullName");
            ViewBag.ClassName = new SelectList(db.ClassLists.OrderBy(t => t.ClassName), "ClassName", "ClassName");

            return View();
        }

        // POST: ClassLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ClassName,FormMaster,StaffNumber")] string TeacherFiter, ClassList classList)
        {

            ClassList Class = db.ClassLists.FirstOrDefault(c => c.ClassName == classList.ClassName);

            if (Class == null)
            {
                //getting and saving Form Master by Id
                int teacherId = Convert.ToInt32(TeacherFiter);
                Teachers teacher = db.Teachers.FirstOrDefault(t => t.Id == teacherId);
                classList.FormMaster = teacher.FullName;
                classList.StaffNumber = teacher.StaffNumber;

                //saving records
                db.ClassLists.Add(classList);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");


            }
            else
            {
                ViewBag.nodetails = "Class and Form Master Already Exist, Delete this Class and Try Again";

            }
            ViewBag.TeacherFiter = new SelectList(db.Teachers, "Id", "FullName");
            ViewBag.ClassName = new SelectList(db.ClassLists.OrderBy(t => t.ClassName), "ClassName", "ClassName");
            return View(classList);
        }

        //GET: ClassLists/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.FormMaster = new SelectList(db.Teachers.OrderBy(t => t.FullName), "FullName", "FullName");
            //ViewBag.ClassName = new SelectList(db.ClassLists.OrderBy(t => t.ClassName), "ClassName", "ClassName");



            ViewBag.TeacherFiter = new SelectList(db.Teachers, "Id", "FullName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassList classList = db.ClassLists.Find(id);
            if (classList == null)
            {
                return HttpNotFound();
            }
            return View(classList);
        }

        //POST: ClassLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ClassName,FormMaster,StaffNumber")] ClassList classList)
        {
            if (ModelState.IsValid)
            {
                //getting staff number 
                Teachers teacher = db.Teachers.FirstOrDefault(t => t.FullName == classList.FormMaster);
                classList.StaffNumber = teacher.StaffNumber;

                db.Entry(classList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            ViewBag.FormMaster = new SelectList(db.Teachers.OrderBy(t => t.FullName), "FullName", "FullName");
            //ViewBag.ClassName = new SelectList(db.ClassLists.OrderBy(t => t.ClassName), "ClassName", "ClassName");
            return View(classList);
        }

        // GET: ClassLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassList classList = db.ClassLists.Find(id);
            if (classList == null)
            {
                return HttpNotFound();
            }
            return View(classList);
        }

        // POST: ClassLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassList classList = db.ClassLists.Find(id);
            db.ClassLists.Remove(classList);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
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
