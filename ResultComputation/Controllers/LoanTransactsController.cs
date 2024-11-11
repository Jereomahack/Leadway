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
    public class LoanTransactsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LoanTransacts
        public ActionResult Index()
        {
            return View(db.LoanTransacts.ToList());
        }

        // GET: LoanTransacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoanTransact loanTransact = db.LoanTransacts.Find(id);
            if (loanTransact == null)
            {
                return HttpNotFound();
            }
            return View(loanTransact);
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
