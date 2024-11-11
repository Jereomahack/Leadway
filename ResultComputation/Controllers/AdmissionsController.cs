using LightWay.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class AdmissionsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admissions
        public async Task<ActionResult> Index()
        {


            return View(await db.admissions.ToListAsync());
        }




        // GET: Admissions/Details/5
        public async Task<ActionResult> Details(int? id)

        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admission admission = await db.admissions.FindAsync(id);



            //CHECKING Logo
            SchoolLogo logo = db.SchoolLogoes.FirstOrDefault();
            if (logo != null)
            {
                ViewBag.Logo = logo.logo;
            }

            ViewBag.Passport = admission.Passport;
            if (admission == null)
            {
                return HttpNotFound();
            }
            return View(admission);
        }

        // GET: Admissions/Create
        public ActionResult Create()
        {
            ViewBag.StateOfOrigin = new SelectList(db.state, "StateName", "StateName");
            ViewBag.Lga = new SelectList("", "", "");


            //CHECKING Logo
            SchoolLogo logo = db.SchoolLogoes.FirstOrDefault();
            if (logo != null)
            {
                ViewBag.Logo = logo.logo;
            }
            return View();
        }

        // POST: Admissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FormNo,DOB,Status,Surname,Firstname,Middlename,Gender,DateTime,Passport,Allergic,PoB,StateOfOrigin,Lga,Nationality,Religion,Language,Psa,Mtitle,MsurnName,Mrelationships,Mtelephone,Maddress,Mgsmnumber,MemailAddress,Moccupation,Memployer,Ftitle,FsurnName,Frelationships,Ftelephone,Faddress,Fgsmnumber,FemailAddress,Foccupation,Femployer,Discovery,DateRecieved,Dateofbirth,DateAssessment,YearGroup,Place,Startingdate")] Admission admission, HttpPostedFileBase File)
        {
            ViewBag.StateOfOrigin = new SelectList(db.state, "StateName", "StateName");
            ViewBag.Lga = new SelectList(db.lgas, "LGAName", "LGAName");

            //if (ModelState.IsValid)
            //{
            //Generating Form Number
            var bytess = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytess);
            int FormNo = BitConverter.ToInt32(bytess, 0) % 1000000;
            var abs = Math.Abs(FormNo);
            admission.FormNo = abs.ToString();

            string filename = "";
            byte[] bytes;
            int BytestoRead;
            int numBytesRead;

            if (File != null && File.ContentLength > 0)
            {
                //Save Image to DB as Byte
                filename = Path.GetFileName(File.FileName);

                bytes = new byte[File.ContentLength];

                BytestoRead = File.ContentLength;

                numBytesRead = 0;

                while (BytestoRead > 0)
                {

                    int n = File.InputStream.Read(bytes, numBytesRead, BytestoRead);

                    if (n == 0) break;

                    numBytesRead += n;

                    BytestoRead -= n;

                }

                admission.Passport = bytes;

            }


            admission.DateTime = DateTime.Now.Date;
            db.admissions.Add(admission);
            await db.SaveChangesAsync();
            return RedirectToAction("details", new { id = admission.Id });
            //}

            //return View(admission);


        }

        // GET: Admissions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admission admission = await db.admissions.FindAsync(id);
            if (admission == null)
            {
                return HttpNotFound();
            }
            return View(admission);
        }

        // POST: Admissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FormNo,Status,Surname,Firstname,Middlename,Gender,DateTime,Passport,Allergic,PoB,SoO,Lga,Nationality,Religion,Language,Psa,Mtitle,MsurnName,Mrelationships,Mtelephone,Maddress,Mgsmnumber,MemailAddress,Moccupation,Memployer,Ftitle,FsurnName,Frelationships,Ftelephone,Faddress,Fgsmnumber,FemailAddress,Foccupation,Femployer,Discovery,DateRecieved,Dateofbirth,DateAssessment,YearGroup,Place,Startingdate")] Admission admission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admission).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(admission);
        }

        // GET: Admissions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admission admission = await db.admissions.FindAsync(id);
            if (admission == null)
            {
                return HttpNotFound();
            }
            return View(admission);
        }

        // POST: Admissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Admission admission = await db.admissions.FindAsync(id);
            db.admissions.Remove(admission);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public JsonResult GetLGA(string StateId)
        {
            if (StateId != null)
            {
                State getstate = db.state.FirstOrDefault(t => t.StateName == StateId);

                var lgas = db.lgas.Where(t => t.StateId == getstate.Id).ToList();
                if (Request.IsAjaxRequest())
                {
                    return new JsonResult
                    {
                        Data = lgas,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }

            return new JsonResult
            {
                Data = "",
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };
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
