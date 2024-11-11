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
    public class CodeGeneratorsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CodeGenerators
        public ActionResult Index(string Phone, string Message)
        {
            if (Phone == null)
            {
                ViewBag.Detail = Message;
                return View(db.CodeGenerators.ToList());
            }
            else if (Phone == "")
            {
                return View(db.CodeGenerators.ToList());
            }
            else
            {
                return View(db.CodeGenerators.ToList().Where(t => t.PhoneNumber == Phone.Trim()));
            }
        }

        // GET: CodeGenerators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeGenerator codeGenerator = db.CodeGenerators.Find(id);
            if (codeGenerator == null)
            {
                return HttpNotFound();
            }
            return View(codeGenerator);
        }

        // GET: CodeGenerators/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CodeGenerators/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PhoneNumber,codes,Parent")] CodeGenerator codeGenerator)
        {
            if (ModelState.IsValid)
            {
                CodeGenerator findcode = db.CodeGenerators.FirstOrDefault(r => r.PhoneNumber == codeGenerator.PhoneNumber);

                if (findcode == null)
                {

                    Student student = db.Students.FirstOrDefault(t => t.PhoneNumber == codeGenerator.PhoneNumber);
                    if (student != null)
                    {
                        //Generating Random code
                        string strPwdchar = "1234567890125357091134567897875678909896789990987";
                        string strPwd = "";
                        Random rnd = new Random();
                        for (int p = 0; p <= 5; p++)
                        {
                            int pRandom = rnd.Next(0, strPwdchar.Length - 1);
                            strPwd += strPwdchar.Substring(pRandom, 1);
                        }

                        codeGenerator.codes = strPwd;

                        //SAVING PARENT NAME
                        codeGenerator.Parent = student.GuardianName;

                        //saving to Parent Code Table
                        ParentCode parentcode = db.ParentCodes.FirstOrDefault(t => t.PhoneNumber == codeGenerator.PhoneNumber);
                        if (parentcode == null)
                        {
                            ParentCode code = new ParentCode()
                            {
                                PhoneNumber = codeGenerator.PhoneNumber,
                                codes = codeGenerator.codes
                            };
                            db.ParentCodes.Add(code);
                            db.SaveChanges();
                        }
                        else
                        {
                            parentcode.codes = codeGenerator.codes;
                            parentcode.PhoneNumber = codeGenerator.PhoneNumber;

                            db.Entry(codeGenerator).State = EntityState.Modified;
                            db.SaveChanges();

                        }

                        db.CodeGenerators.Add(codeGenerator);
                        db.SaveChanges();


                        string Message = "Code Generated for " + codeGenerator.Parent + " " + " is: " + codeGenerator.codes;
                        return RedirectToAction("Index", new { Message });
                    }
                    else
                    {
                        ViewBag.NoDetail = "This Phone does not exist for any of the student in the school, Please ensure the right Phone Number was entered";
                        return View();
                    }
                }
                else
                {
                    ViewBag.NoDetail = "Code has already been generated for this parent: " + findcode.Parent + " " + " The Code is: " + findcode.codes;
                    return View();
                }
            }

            return View(codeGenerator);
        }


        //Get: Create Parent Login
        public ActionResult ParentLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ParentLogin([Bind(Include = "Id,PhoneNumber,codes,Parent")] CodeGenerator codeGenerator)
        {
            //check if any student uses such phone number
            Student checkstudnt = db.Students.FirstOrDefault(t => t.PhoneNumber == codeGenerator.PhoneNumber);
            if (checkstudnt != null)
            {

            }

            return View();
        }

        // GET: CodeGenerators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeGenerator codeGenerator = db.CodeGenerators.Find(id);
            if (codeGenerator == null)
            {
                return HttpNotFound();
            }
            return View(codeGenerator);
        }

        // POST: CodeGenerators/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PhoneNumber,codes,Parent")] CodeGenerator codeGenerator)
        {
            if (ModelState.IsValid)
            {

                CodeGenerator code = db.CodeGenerators.Find(codeGenerator.Id);

                ParentCode pcode = db.ParentCodes.FirstOrDefault(r => r.PhoneNumber == code.PhoneNumber);
                if (pcode != null)
                {
                    pcode.PhoneNumber = codeGenerator.PhoneNumber;

                    db.Entry(pcode).State = EntityState.Modified;
                    db.SaveChanges();
                }


                db.Entry(code).CurrentValues.SetValues(codeGenerator);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(codeGenerator);
        }

        // GET: CodeGenerators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CodeGenerator codeGenerator = db.CodeGenerators.Find(id);
            if (codeGenerator == null)
            {
                return HttpNotFound();
            }
            return View(codeGenerator);
        }

        // POST: CodeGenerators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CodeGenerator codeGenerator = db.CodeGenerators.Find(id);
            db.CodeGenerators.Remove(codeGenerator);
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
