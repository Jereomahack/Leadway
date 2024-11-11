using LightWay.Models;
using ResultComputation.Models;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class SchoolLogoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SchoolLogoes
        public async Task<ActionResult> Index()
        {
            return View(await db.SchoolLogoes.ToListAsync());
        }

        // GET: SchoolLogoes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolLogo schoolLogo = await db.SchoolLogoes.FindAsync(id);
            if (schoolLogo == null)
            {
                return HttpNotFound();
            }
            return View(schoolLogo);
        }

        // GET: SchoolLogoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SchoolLogoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,logo")] SchoolLogo schoolLogo, HttpPostedFileBase file)
        {

            string filename = "";

            byte[] bytes;

            int BytestoRead;

            int numBytesRead;

            if (ModelState.IsValid)
            {

                if (file != null && file.ContentLength > 0)
                {
                    //Save Image to DB as Byte
                    filename = Path.GetFileName(file.FileName);

                    bytes = new byte[file.ContentLength];

                    BytestoRead = file.ContentLength;

                    numBytesRead = 0;

                    while (BytestoRead > 0)
                    {

                        int n = file.InputStream.Read(bytes, numBytesRead, BytestoRead);

                        if (n == 0) break;

                        numBytesRead += n;

                        BytestoRead -= n;

                    }

                    schoolLogo.logo = bytes;

                }

                db.SchoolLogoes.Add(schoolLogo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(schoolLogo);
        }

        // GET: SchoolLogoes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolLogo schoolLogo = await db.SchoolLogoes.FindAsync(id);
            if (schoolLogo == null)
            {
                return HttpNotFound();
            }
            return View(schoolLogo);
        }

        // POST: SchoolLogoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,logo")] SchoolLogo schoolLogo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schoolLogo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(schoolLogo);
        }

        // GET: SchoolLogoes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SchoolLogo schoolLogo = await db.SchoolLogoes.FindAsync(id);
            if (schoolLogo == null)
            {
                return HttpNotFound();
            }
            return View(schoolLogo);
        }

        // POST: SchoolLogoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SchoolLogo schoolLogo = await db.SchoolLogoes.FindAsync(id);
            db.SchoolLogoes.Remove(schoolLogo);
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
