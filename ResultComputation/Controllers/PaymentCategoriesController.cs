using LightWay.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class PaymentCategoriesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaymentCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.PaymentCategories.ToListAsync());
        }

        // GET: PaymentCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentCategory paymentCategory = await db.PaymentCategories.FindAsync(id);
            if (paymentCategory == null)
            {
                return HttpNotFound();
            }
            return View(paymentCategory);
        }

        // GET: PaymentCategories/Create
        public ActionResult Create()
        {
            ViewBag.ClassName = new SelectList(db.ClassLists.OrderBy(t => t.ClassName), "ClassName", "ClassName");
            return View();
        }

        // POST: PaymentCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CategoryName,ClassName")] PaymentCategory paymentCategory)
        {
            if (ModelState.IsValid)
            {
                db.PaymentCategories.Add(paymentCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paymentCategory);
        }

        // GET: PaymentCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentCategory paymentCategory = await db.PaymentCategories.FindAsync(id);
            if (paymentCategory == null)
            {
                return HttpNotFound();
            }
            return View(paymentCategory);
        }

        // POST: PaymentCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CategoryName,ClassName")] PaymentCategory paymentCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paymentCategory);
        }

        // GET: PaymentCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentCategory paymentCategory = await db.PaymentCategories.FindAsync(id);
            if (paymentCategory == null)
            {
                return HttpNotFound();
            }
            return View(paymentCategory);
        }

        // POST: PaymentCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PaymentCategory paymentCategory = await db.PaymentCategories.FindAsync(id);
            db.PaymentCategories.Remove(paymentCategory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult getsub(string Cat)
        {

            return RedirectToAction("SubList", "PaymentSubCategories", new { Cat });
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
