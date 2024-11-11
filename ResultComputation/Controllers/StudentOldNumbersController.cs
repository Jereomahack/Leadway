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
    public class StudentOldNumbersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentOldNumbers
        public ActionResult Index(string Name)
        {
            if(Name==null)
            {
                return View(db.OldAdmissionNumbers.ToList());
            }
            else
            {
                return View(db.OldAdmissionNumbers.ToList().Where(r=>r.StudentName.Contains(Name)));
            }
        }

    
        // GET: StudentOldNumbers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentOldNumbers studentOldNumbers = db.OldAdmissionNumbers.Find(id);
            if (studentOldNumbers == null)
            {
                return HttpNotFound();
            }
            return View(studentOldNumbers);
        }

        // POST: StudentOldNumbers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentOldNumbers studentOldNumbers = db.OldAdmissionNumbers.Find(id);
            db.OldAdmissionNumbers.Remove(studentOldNumbers);
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
