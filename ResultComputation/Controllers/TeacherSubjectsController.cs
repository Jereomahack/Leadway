using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LightWay.Models;
using System.Web.Security;
using System.Threading.Tasks;
using System.Net.Mail;

namespace LightWay.Controllers
{
    public class TeacherSubjectsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TeacherSubjects
        [Authorize(Roles = "Admin,Teacher")]
        public ActionResult Index()
        {

            return View(db.TeacherSubjects.ToList());
        }

        // GET: TeacherSubjects/Details/5

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TeacherSubject teacherSubject = db.TeacherSubjects.Find(id);
        //    if (teacherSubject == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(teacherSubject);
        //}

        // GET: TeacherSubjects/Create

        [Authorize(Roles = "Admin")]
        public ActionResult Create(int id)
        {

            Teachers teacher = db.Teachers.Find(id);
            if (teacher != null)
            {
                ViewBag.TeacherNum = teacher.StaffNumber;
                ViewBag.TeacherName = teacher.FullName;
                ViewBag.TeacherEmail = teacher.EmailAddress;
                ViewBag.Subject = new SelectList(db.SchoolSubjectss.OrderBy(r=>r.Subject), "Id", "Subject");
                ViewBag.Class = new SelectList(db.ClassLists.OrderBy(t=>t.ClassName), "Id", "ClassName");
            }

            TempData["TeacherNum"] = ViewBag.TeacherNum;
            TempData["TeacherName"] = ViewBag.TeacherName;
            TempData["TeacherEmail"] = ViewBag.TeacherEmail;

            return View(db.TeacherSubjects.ToList().Where(r => r.TeacherNum == teacher.StaffNumber));
        }

        // POST: TeacherSubjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,TeacherNum,TeacherName,Subject,Class")] TeacherSubject teacherSubject)
        {


            //saving records into Db
            teacherSubject.TeacherNum = Convert.ToString(TempData["TeacherNum"]);
            teacherSubject.TeacherName = Convert.ToString(TempData["TeacherName"]);
            teacherSubject.EmailAddress = Convert.ToString(TempData["TeacherEmail"]);
            teacherSubject.CreatedBy = User.Identity.Name;
            teacherSubject.DateRecorded = DateTime.Now;

            //getting Subject and Class By Id
            int subjectID = Convert.ToInt32(teacherSubject.Subject);
            int classId = Convert.ToInt32(teacherSubject.Class);
            SchoolSubjects subject = db.SchoolSubjectss.FirstOrDefault(s => s.Id == subjectID);
            ClassList Class = db.ClassLists.FirstOrDefault(c => c.Id == classId);

            if (subject != null && Class != null)
            {
                teacherSubject.Subject = subject.Subject;
                teacherSubject.Class = Class.ClassName;
            }
            else if (subject != null && Class == null)
            {
                teacherSubject.Subject = subject.Subject;
                teacherSubject.Class = null;
            }
            else
            {
                teacherSubject.Subject = null;
                teacherSubject.Class = Class.ClassName;
            }

            db.TeacherSubjects.Add(teacherSubject);
            db.SaveChanges();


            ViewBag.Subject = new SelectList(db.SchoolSubjectss, "Id", "Subject");
            ViewBag.Class = new SelectList(db.ClassLists, "Id", "ClassName");
            return RedirectToAction("Create");

        }

        // GET: TeacherSubjects/Edit/5

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherSubject teacherSubject = db.TeacherSubjects.Find(id);
            if (teacherSubject == null)
            {
                return HttpNotFound();
            }

            return View(teacherSubject);
        }

        // POST: TeacherSubjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,TeachersId,SubjectsId,ClassListId")] TeacherSubject teacherSubject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacherSubject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(teacherSubject);
        }

        // GET: TeacherSubjects/Delete/5

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherSubject teacherSubject = db.TeacherSubjects.Find(id);
            if (teacherSubject == null)
            {
                return HttpNotFound();
            }
            return View(teacherSubject);
        }

        // POST: TeacherSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            TeacherSubject teacherSubject = db.TeacherSubjects.Find(id);
            db.TeacherSubjects.Remove(teacherSubject);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Remove(int id)
        {

            TeacherSubject teachersubject = db.TeacherSubjects.Find(id);

            string StaffNum = teachersubject.TeacherNum;
            var teacherId = db.Teachers.FirstOrDefault(t => t.StaffNumber == StaffNum);
            int Id = teacherId.Id;

            if (teachersubject == null)
            {
                return RedirectToAction("Create", new { Id });

            }

            db.TeacherSubjects.Remove(teachersubject);
            db.SaveChanges();
            return RedirectToAction("Create", new { Id });
        }


        [Authorize(Roles = "Teacher")]
        public ActionResult SubTeacher()
        {
            //getting User
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            return View(db.TeacherSubjects.ToList().Where(c => c.TeacherNum == user.StaffNumber));
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
