using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LightWay.Models;
using System.IO;

namespace LightWay.Controllers
{
    public class ParentCodesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ParentCodes
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string search, string message)
        {
            ViewBag.Message = message;

            if (search != null)
            {
                return View(db.ParentCodes.ToList().Where(r => r.PhoneNumber == search));
            }
            return View(db.ParentCodes.ToList());
        }

        // GET: ParentCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ParentCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PhoneNumber,codes")] ParentCode parentCode)
        {
            if (ModelState.IsValid)
            {

                if (parentCode.PhoneNumber != null && parentCode.codes == null)
                {
                    Student std = db.Students.FirstOrDefault(d => d.PhoneNumber == parentCode.PhoneNumber);
                    if (std != null)
                    {
                        ParentCode codes = db.ParentCodes.FirstOrDefault(r => r.PhoneNumber == std.PhoneNumber);

                        if (codes != null)
                        {
                            //sendind sms to parent
                            string message = "To check your child result, enter this code " + codes.codes + " and phone No used 4 his/her admission" + "  on http://lightway.osoftint.com/parentcodes/create Thanks";
                            string pwd = "smispwd12345";
                            string YourUsername = "OSOFTSMIS";
                            string sendto = codes.PhoneNumber;
                            string senderid = "LIGHTWAY";
                            string sURL;
                            StreamReader objReader;
                            sURL = "http://api.smartsmssolutions.com/smsapi.php?username=" + YourUsername + "&password=" + pwd + "&message=" + message + "&sender=" + senderid + "&recipient=" + sendto;
                            WebRequest wrGETURL;
                            wrGETURL = WebRequest.Create(sURL);
                            try
                            {
                                Stream objStream;
                                objStream = wrGETURL.GetResponse().GetResponseStream();
                                objReader = new StreamReader(objStream);
                                objReader.Close();
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }

                            return RedirectToAction("Create");
                        }
                        else
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

                            parentCode.codes = strPwd;

                            db.ParentCodes.Add(parentCode);
                            db.SaveChanges();

                            //sendind sms to parent
                            string message = "To check your child result, enter this code " + codes.codes + " and phone No used 4 his/her admission" + "  on http://lightway.osoftint.com/parentcodes/create Thanks";
                            string pwd = "smispwd12345";
                            string YourUsername = "OSOFTSMIS";
                            string sendto = codes.PhoneNumber;
                            string senderid = "LIGHTWAY";
                            string sURL;
                            StreamReader objReader;
                            sURL = "http://api.smartsmssolutions.com/smsapi.php?username=" + YourUsername + "&password=" + pwd + "&message=" + message + "&sender=" + senderid + "&recipient=" + sendto;
                            WebRequest wrGETURL;
                            wrGETURL = WebRequest.Create(sURL);
                            try
                            {
                                Stream objStream;
                                objStream = wrGETURL.GetResponse().GetResponseStream();
                                objReader = new StreamReader(objStream);
                                objReader.Close();
                            }
                            catch (Exception ex)
                            {
                                ex.ToString();
                            }

                            ViewBag.NoDetail = "Please enter enter Your Phone Number and the Login Code sent to your Phone to check your child result. Thank You";
                            return RedirectToAction("Create");
                        }
                    }
                    else
                    {
                        // Generate VIewbag Message
                        return RedirectToAction("Create");
                    }

                }
                else if (parentCode.codes != null && parentCode.PhoneNumber != null)
                {
                    Student std = db.Students.FirstOrDefault(d => d.PhoneNumber == parentCode.PhoneNumber);
                    if (std != null)
                    {
                        ParentCode codes = db.ParentCodes.FirstOrDefault(r => r.PhoneNumber == std.PhoneNumber && r.codes == parentCode.codes);

                        if (codes != null)
                        {

                            //getting Phone Number
                            string phone = std.PhoneNumber;

                            return RedirectToAction("CheckResult", "scores", new { phone });
                        }
                        else
                        {
                            ViewBag.NoDetail = "Please ensure this Number :" + parentCode.PhoneNumber + " " + "was the Number Provider for your child admission into the school"
                                + "Also ensure the code entered was the right code. If not sure enter phone number and click proceed, a code will be sent to your phone " + "";
                            return RedirectToAction("Create");
                        }
                    }
                    else
                    {
                        ViewBag.NoDetail = "Please ensure this Number :" + parentCode.PhoneNumber + " " + "was the Number Provider for your child admission into the school"
                               + "Also ensure the code entered was the right code. If not sure enter phone number and click proceed, a code will be sent to your phone " + "";
                        return RedirectToAction("Create");
                    }
                }
                else
                {
                    //Do nothing or use Viewbag Message
                }

                return RedirectToAction("Index");
            }

            return View(parentCode);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ParentCode parentcode = db.ParentCodes.Find(id);

            //getting parent/Guardian detail from student information
            Student student = db.Students.FirstOrDefault(t => t.PhoneNumber == parentcode.PhoneNumber);
            if (student != null)
            {
                ViewBag.ParentName = student.GuardianName;
                ViewBag.Phone = parentcode.PhoneNumber;
                ViewBag.Code = parentcode.codes;
            }



            if (parentcode == null)
            {
                return HttpNotFound();
            }
            return View(parentcode);
        }

        public ActionResult Message()
        {
            foreach (var item in db.Students.Where(t => t.Transistion == "No").ToList())
            {

                //getting the code from parent code
                ParentCode getcode = db.ParentCodes.FirstOrDefault(t => t.PhoneNumber == item.PhoneNumber);

                if (getcode != null)
                {
                    //sendind sms to parent
                    string Message = "To check your child result, enter this code " + getcode.codes + " and phone No used 4 his/her admission" + "  on http://lightwayacademy.gear.host/Parentcodes/create Thanks";
                    string pwd = "smispwd12345";
                    string YourUsername = "OSOFTSMIS";
                    string sendto = getcode.PhoneNumber;
                    string senderid = "LIGHTWAY";
                    string sURL;
                    StreamReader objReader;
                    sURL = "http://api.smartsmssolutions.com/smsapi.php?username=" + YourUsername + "&password=" + pwd + "&message=" + Message + "&sender=" + senderid + "&recipient=" + sendto;
                    WebRequest wrGETURL;
                    wrGETURL = WebRequest.Create(sURL);
                    try
                    {
                        Stream objStream;
                        objStream = wrGETURL.GetResponse().GetResponseStream();
                        objReader = new StreamReader(objStream);
                        objReader.Close();
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                }

            }
            string message = "Message successfully sent";

            return RedirectToAction("Index", new { message });
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
