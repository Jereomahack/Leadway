using LightWay.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ResultComputation.Controllers
{
    public class ExpTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ExpTypes
        public async Task<ActionResult> Index()
        {
            return PartialView(await db.exptypes.ToListAsync());
        }

        // GET: ExpTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpType expType = await db.exptypes.FindAsync(id);
            if (expType == null)
            {
                return HttpNotFound();
            }
            return View(expType);
        }

        // GET: ExpTypes/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: ExpTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ExpensesName")] ExpType expType)
        {
            if (ModelState.IsValid)
            {
                db.exptypes.Add(expType);
                await db.SaveChangesAsync();
                string message = expType.ExpensesName + " was successfully added. Thank You.";
                return Json(new { message });
            }

            return View(expType);
        }

        // GET: ExpTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpType expType = await db.exptypes.FindAsync(id);
            if (expType == null)
            {
                return HttpNotFound();
            }
            return View(expType);
        }

        // POST: ExpTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ExpensesName")] ExpType expType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(expType);
        }

        // GET: ExpTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExpType expType = await db.exptypes.FindAsync(id);
            if (expType == null)
            {
                return HttpNotFound();
            }
            return View(expType);
        }

        // POST: ExpTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ExpType expType = await db.exptypes.FindAsync(id);
            db.exptypes.Remove(expType);
            await db.SaveChangesAsync();
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
