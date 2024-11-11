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
    public class TransactionInventoriesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TransactionInventories
        public ActionResult Index()
        {
            return View(db.TransactionInventories.ToList());
        }

        // GET: TransactionInventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransactionInventory transactionInventory = db.TransactionInventories.Find(id);
            if (transactionInventory == null)
            {
                return HttpNotFound();
            }
            return View(transactionInventory);
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
