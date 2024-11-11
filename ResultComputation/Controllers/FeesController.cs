using LightWay.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class FeesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Fees
        [Authorize(Roles = "Admin, Accountant, Principal, ")]
        public ActionResult Index(string Ref, string SearchFrom, string SearchTo, string Statues, string Phone, string Message, string Session, string Term)
        {
            ViewBag.Message = Message;
            ViewBag.Term = new SelectList(db.terms, "TermName", "TermName");
            ViewBag.Session = new SelectList(db.Sessions, "AcademicYear", "AcademicYear");
            if (Ref != null && SearchFrom != null && SearchTo != null && Statues != null && Phone != null && Session != null && Term != null)
            {
                if (Ref != "" && SearchFrom == "" && SearchTo == "" && Statues == "" && Phone == "" && Session == "" && Term == "")
                {
                    return View(db.Fees.Where(t => t.TransactionRef == Ref).ToList());
                }
                else if (Ref == "" && SearchFrom == "" && SearchTo == "" && Statues == "" && Phone != "" && Session == "" && Term == "")
                {
                    return View(db.Fees.Where(t => t.PhoneNumber == Phone).ToList());
                }
                else if (Ref == "" && SearchFrom == "" && SearchTo == "" && Statues != "" && Phone == "" && Session == "" && Term == "")
                {

                    return View(db.Fees.Where(t => t.PaymentStatues.Contains(Statues)).ToList());
                }
                else if (Ref == "" && SearchFrom != "" && SearchTo != "" && Statues == "" && Phone == "" && Session == "" && Term == "")
                {
                    var datefrom = Convert.ToDateTime(SearchFrom);
                    var dateto = Convert.ToDateTime(SearchTo);
                    var payment = from d in db.Fees
                                  where d.PaymentDate >= datefrom && d.PaymentDate <= dateto && d.PaymentStatues.Contains("Approved")
                                  select d;
                    if (payment.Any())
                    {
                        ViewBag.Total = payment.Sum(t => t.amount).ToString("###,###.00");
                    }
                    else
                    {
                        ViewBag.Fail = "No Result Found";
                    }
                    return View(payment.ToList());
                }
                else if (Ref == "" && SearchFrom != "" && SearchTo != "" && Statues != "" && Phone == "" && Session == "" && Term == "")
                {
                    var datefrom = Convert.ToDateTime(SearchFrom);
                    var dateto = Convert.ToDateTime(SearchTo);
                    var payment = from d in db.Fees
                                  where d.PaymentDate >= datefrom && d.PaymentDate <= dateto && d.PaymentStatues.Contains(Statues)
                                  select d;
                    if (payment.Any())
                    {
                        ViewBag.Total = payment.Sum(t => t.amount).ToString("###,###.00");
                    }
                    else
                    {
                        ViewBag.Fail = "No Result Found";
                    }
                    return View(payment.ToList());
                }
                else if (Ref == "" && SearchFrom != "" && SearchTo != "" && Statues == "" && Phone != "" && Session == "" && Term == "")
                {
                    var datefrom = Convert.ToDateTime(SearchFrom);
                    var dateto = Convert.ToDateTime(SearchTo);
                    var payment = from d in db.Fees
                                  where d.PaymentDate >= datefrom && d.PaymentDate <= dateto && d.PhoneNumber == (Phone)
                                  select d;
                    if (payment.Any())
                    {
                        ViewBag.Total = payment.Sum(t => t.amount).ToString("###,###.00");
                    }
                    else
                    {
                        ViewBag.Fail = "No Result Found";
                    }
                    return View(payment.ToList());
                }

                else if (Ref == "" && SearchFrom != "" && SearchTo != "" && Statues == "" && Phone == "" && Session != "" && Term != "")
                {
                    var datefrom = Convert.ToDateTime(SearchFrom);
                    var dateto = Convert.ToDateTime(SearchTo);
                    var payment = from d in db.Fees
                                  where d.PaymentDate >= datefrom && d.PaymentDate <= dateto && d.AccSession == Session && d.AccTerm == Term && d.PaymentStatues.Contains("Approved")
                                  select d;
                    if (payment.Any())
                    {
                        ViewBag.Total = payment.Sum(t => t.amount).ToString("###,###.00");
                    }
                    else
                    {
                        ViewBag.Fail = "No Result Found";
                    }
                    return View(payment.ToList());
                }
                else if (Ref == "" && SearchFrom != "" && SearchTo != "" && Statues == "" && Phone == "" && Session != "" && Term == "")
                {
                    var datefrom = Convert.ToDateTime(SearchFrom);
                    var dateto = Convert.ToDateTime(SearchTo);
                    var payment = from d in db.Fees
                                  where d.PaymentDate >= datefrom && d.PaymentDate <= dateto && d.AccSession == Session && d.PaymentStatues.Contains("Approved")
                                  select d;
                    if (payment.Any())
                    {
                        ViewBag.Total = payment.Sum(t => t.amount).ToString("###,###.00");
                    }
                    else
                    {
                        ViewBag.Fail = "No Result Found";
                    }
                    return View(payment.ToList());
                }
                else if (Ref == "" && SearchFrom == "" && SearchTo == "" && Statues == "" && Phone == "" && Session != "" && Term != "")
                {

                    var payment = from d in db.Fees
                                  where d.AccSession == Session && d.AccTerm == Term && d.PaymentStatues.Contains("Approved")
                                  select d;
                    if (payment.Any())
                    {
                        ViewBag.Total = payment.Sum(t => t.amount).ToString("###,###.00");
                    }
                    else
                    {
                        ViewBag.Fail = "No Result Found";
                    }
                    return View(payment.ToList());
                }
                else if (Ref == "" && SearchFrom == "" && SearchTo == "" && Statues == "" && Phone == "" && Session != "" && Term == "")
                {

                    var payment = from d in db.Fees
                                  where d.AccSession == Session && d.PaymentStatues.Contains("Approved")
                                  select d;
                    if (payment.Any())
                    {
                        ViewBag.Total = payment.Sum(t => t.amount).ToString("###,###.00");
                    }
                    else
                    {
                        ViewBag.Fail = "No Result Found";
                    }
                    return View(payment.ToList());
                }
                else if (Ref != "" && SearchFrom == "" && SearchTo == "" && Statues == "" && Phone == "" && Session == "" && Term == "")
                {
                    return View(db.Fees.Where(t => t.TransactionRef == Ref).ToList());
                }
            }

            return View(db.Fees.OrderByDescending(t => t.PaymentDate).Where(t => t.PaymentStatues.Contains("Approved")).ToList().Take(100));
        }

        // GET: Fees
        [Authorize(Roles = "Parent")]
        public ActionResult IndexParent(string SearchFrom, string SearchTo, string Ref)
        {

            if (SearchFrom != null && SearchTo != null)
            {

                if (SearchFrom != "" && SearchTo != "")
                {
                    var datefrom = Convert.ToDateTime(SearchFrom);
                    var dateto = Convert.ToDateTime(SearchTo);
                    var payment = from d in db.Fees
                                  where d.PaymentDate >= datefrom && d.PaymentDate <= dateto && d.PaymentStatues.Contains("Approved") && d.PhoneNumber == User.Identity.Name
                                  select d;
                    if (payment.Any())
                    {
                        ViewBag.Total = payment.Sum(t => t.amount).ToString("###,###.00");
                    }
                    else
                    {
                        ViewBag.Fail = "No Result Found";
                    }
                    return View(payment.OrderByDescending(t => t.PaymentDate).ToList());
                }
            }
            else if (Ref != null)
            {
                ViewBag.Mesage = "This transaction Already exist, please click on the verify or Try again button if payment is pending. Thank You.";
                return View(db.Fees.Where(t => t.TransactionRef == Ref).ToList());
            }

            return View(db.Fees.Where(t => t.PhoneNumber == User.Identity.Name).OrderByDescending(t => t.PaymentDate).ToList().Take(20));
        }

        // GET: Fees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fee fee = await db.Fees.FindAsync(id);
            if (fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }

        // GET: Fees/Create
        public ActionResult Create(int? id, string Ref, string Message, string PayType)
        {
            ViewBag.Message = Message;

            //get student detail
            Student stdnt = db.Students.FirstOrDefault(t => t.PhoneNumber == User.Identity.Name);
            ViewBag.Email = stdnt.EmailAddress;

            ViewBag.StudentReg = db.Students.Where(t => t.PhoneNumber == User.Identity.Name).Select(x => new SelectListItem { Text = x.StudentNumber, Value = x.StudentNumber }).ToList();
           
            ViewBag.AccTerm = new SelectList(db.terms.OrderBy(t => t.Id), "TermName", "TermName");
            ViewBag.AccSession = new SelectList(db.Sessions.OrderByDescending(t => t.Id), "AcademicYear", "AcademicYear");
            return PartialView();
        }

        // POST: Fees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PaymentCat,OPR, PaymentType,PaymentSubCat,StudentReg,StudentName,TransactionRef,AccTerm,AccSession,PaymentDate,amount,PaymentStatues,PhoneNumber,Email,Description")] Fee fee, string Amt, string pubamt)
        {
            //checking payment exist
            fee.amount = Convert.ToDecimal(Amt);
            ViewBag.StudentReg = db.Students.Where(t => t.PhoneNumber == User.Identity.Name).Select(x => new SelectListItem { Text = x.StudentNumber, Value = x.StudentNumber }).ToList();
            Fee checkfee = db.Fees.FirstOrDefault(t => t.PaymentCat == fee.PaymentCat && t.PaymentSubCat == fee.PaymentSubCat && t.StudentReg == fee.StudentReg && t.AccSession == fee.AccSession && t.AccTerm == fee.AccTerm && t.PaymentStatues.Contains("Approved"));
            ViewBag.StudentName = db.Students.Where(t => t.PhoneNumber == User.Identity.Name).Select(x => new SelectListItem { Text = x.Surname + " " + x.OtherName + "(" + x.StudentNumber + ")", Value = x.Surname + " " + x.OtherName + "(" + x.StudentNumber + ")" }).ToList();
            ViewBag.AccTerm = new SelectList(db.terms.OrderBy(t => t.Id), "TermName", "TermName");
            ViewBag.AccSession = new SelectList(db.Sessions.OrderBy(t => t.Id), "AcademicYear", "AcademicYear");

            var PaymentItemName = "TestServiceName1";
            var TransactionType = "2";
            var MerchantNumber = "5gs75a1sh8114a.c";

            //if (checkfee == null)
            //{
            //ViewBag.PaymentCat = new SelectList(db.PaymentCategories.OrderBy(t => t.CategoryName), "CategoryName", "CategoryName");
            //ViewBag.PaymentSubCat = new SelectList("", "", "");

            fee.amount = Convert.ToDecimal(Amt);
            fee.PhoneNumber = User.Identity.Name;
            fee.PaymentDate = DateTime.Now;
            fee.PaymentStatues = "Pending";

            long milliseconds = DateTime.Now.Ticks;
            fee.TransactionRef = milliseconds.ToString();

            if (fee.PaymentType == "Master/Verve/Visa Card")
            {
                //var SiteRedirectURL = "http://localhost:6320/fees/ReturnUrl";
                var SiteRedirectURL = "http://smis.leadwayacademyschool.com/fees/ReturnUrl";

                string hashstring = MerchantNumber + fee.TransactionRef + TransactionType + fee.amount.ToString("###") + SiteRedirectURL;
                string Hash = SHA512(hashstring);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://developer.osoftpay.net/api/TestPublicPayments");
                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("TransactionType", TransactionType),
                new KeyValuePair<string, string>("MerchantNumber", MerchantNumber),
                new KeyValuePair<string, string>("SiteRedirectURL", SiteRedirectURL),
                new KeyValuePair<string, string>("TransactionReference", fee.TransactionRef),
                new KeyValuePair<string, string>("CustomerName", fee.StudentName+"("+fee.StudentReg+")"),
                 new KeyValuePair<string, string>("CustomerId", fee.Email),
                  new KeyValuePair<string, string>("PaymentItemName", PaymentItemName),
                   new KeyValuePair<string, string>("Amount", fee.amount.ToString("###")),
                new KeyValuePair<string, string>("Hash", Hash)
            });
                    var result = client.PostAsync("", content).Result;
                    var url = result.RequestMessage.RequestUri.ToString();

                    db.Fees.Add(fee);
                    await db.SaveChangesAsync();
                    return Redirect(url);

                }
            }
            else if (fee.PaymentType == "Bank branch deposit/Quickteller/Phone USSD")
            {

                db.Fees.Add(fee);
                db.SaveChanges();

                //var SiteRedirectURL = "http://localhost:6320/fees/ServiceUrl";
                var SiteRedirectURL = "http://smis.leadwayacademyschool.com/fees/ServiceUrl";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://developer.osoftpay.net/api/TestBranchPayments");
                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("MerchantNumber", MerchantNumber),
                new KeyValuePair<string, string>("ServiceUrl", SiteRedirectURL),
                new KeyValuePair<string, string>("CustomerName", fee.TransactionRef),
                new KeyValuePair<string, string>("ProcessedBy", fee.Email),
                new KeyValuePair<string, string>("ItemName", PaymentItemName),
                   new KeyValuePair<string, string>("Amount", fee.amount.ToString("###"))
            });
                    var result = client.PostAsync("", content).Result;
                    var url = result.RequestMessage.RequestUri.ToString();
                    return Redirect(url);
                }

            }


            //}
            //else
            //{
            //    string Message = "This Payment has been made. Thank You";
            //    return RedirectToAction("Create", "Fees", new { Message });
            //}

            if (pubamt != null)
            {
                return RedirectToAction("PayBill");
            }
            else
            {
                return View();
            }

        }



        // GET: Fees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fee fee = await db.Fees.FindAsync(id);
            if (fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }

        // POST: Fees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PaymentStatues,Description,PaymentType,OPR,PhoneNumber,Email,PaymentCat,PaymentSubCat,StudentReg,StudentName,TransactionRef,AccTerm,AccSession,PaymentDate,amount")] Fee fee)
        {

            var PaymentItemName = "TestServiceName1";
            var TransactionType = "2";
            var MerchantNumber = "5gs75a1sh8114a.c";
            if (fee.PaymentType == "Master/Verve/Visa Card")
            {
                var SiteRedirectURL = "http://localhost:6320/fees/ReturnUrl";
                //var SiteRedirectURL = "http://smis.staloysiousschoolsedu.ng/fees/ReturnUrl";

                string hashstring = MerchantNumber + fee.TransactionRef + TransactionType + fee.amount.ToString("###") + SiteRedirectURL;
                string Hash = SHA512(hashstring);

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://developer.osoftpay.net/api/TestPublicPayments");
                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("TransactionType", TransactionType),
                new KeyValuePair<string, string>("MerchantNumber", MerchantNumber),
                new KeyValuePair<string, string>("SiteRedirectURL", SiteRedirectURL),
                new KeyValuePair<string, string>("TransactionReference", fee.TransactionRef),
                new KeyValuePair<string, string>("CustomerName", fee.StudentName+"("+fee.StudentReg+")"),
                 new KeyValuePair<string, string>("CustomerId", fee.Email),
                  new KeyValuePair<string, string>("PaymentItemName", PaymentItemName),
                   new KeyValuePair<string, string>("Amount", fee.amount.ToString("###")),
                new KeyValuePair<string, string>("Hash", Hash)
            });
                    var result = client.PostAsync("", content).Result;
                    var url = result.RequestMessage.RequestUri.ToString();


                    //db.Fees.Add(fee);
                    //await db.SaveChangesAsync();
                    return Redirect(url);

                }
            }
            else if (fee.PaymentType == "Bank branch deposit/Quickteller/Phone USSD")
            {
                db.Fees.Add(fee);
                db.SaveChanges();

                var SiteRedirectURL = "http://localhost:6320/fees/ServiceUrl";
                //var SiteRedirectURL = "http://smis.staloysiousschoolsedu.ng/fees/ServiceUrl";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://developer.osoftpay.net/api/TestBranchPayments");
                    var content = new FormUrlEncodedContent(new[]
                    {
                new KeyValuePair<string, string>("MerchantNumber", MerchantNumber),
                new KeyValuePair<string, string>("ServiceUrl", SiteRedirectURL),
                new KeyValuePair<string, string>("CustomerName", fee.TransactionRef),
                new KeyValuePair<string, string>("ProcessedBy", fee.Email),
                new KeyValuePair<string, string>("ItemName", PaymentItemName),
                   new KeyValuePair<string, string>("Amount", fee.amount.ToString("###"))
            });
                    var result = client.PostAsync("", content).Result;
                    var url = result.RequestMessage.RequestUri.ToString();
                    return Redirect(url);
                }

            }
            return View(fee);
        }

        // GET: Fees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fee fee = await db.Fees.FindAsync(id);
            if (fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }

        // POST: Fees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Fee fee = await db.Fees.FindAsync(id);



            db.Fees.Remove(fee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetSub(string Cat, string Reg)
        {

            if (Cat != null)
            {
                ViewBag.Sub = db.PaymentSubCategories.Where(a => a.PaymentCat.Equals(Cat)).OrderBy(a => a.SubCatName).ToList();
                if (Request.IsAjaxRequest())
                {
                    return new JsonResult
                    {
                        Data = ViewBag.Sub,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }
            else if (Reg != null || Reg != "")
            {
                Student getstudent = db.Students.FirstOrDefault(t => t.StudentNumber == Reg);
                if (getstudent != null)
                {

                    ViewBag.Name = getstudent;
                    if (Request.IsAjaxRequest())
                    {
                        return new JsonResult
                        {
                            Data = ViewBag.Name,
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet
                        };
                    }
                }
                else
                {
                    return new JsonResult
                    {
                        Data = "Registration Number not Found, Please ensure the Reg No. is entered. Thank You",
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

            }

            return new JsonResult
            {
                Data = "Not a valid request",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        public string SHA512(string hashstring)
        {
            System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();
            Byte[] EncryptedSHA512 = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hashstring));
            sha512.Clear();
            string hashed = BitConverter.ToString(EncryptedSHA512).Replace("-", "").ToLower();
            return hashed;
        }

        public ActionResult ReturnUrl(string TransactionReference, string PayRef, string ResCode, string ResDesc)
        {

            //updating transaction statues
            Fee fee = db.Fees.FirstOrDefault(t => t.TransactionRef == TransactionReference);
            fee.PaymentStatues = ResDesc;
            fee.Payref = PayRef;
            db.Entry(fee).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Id = fee.Id;
            ViewBag.Transfref = TransactionReference;
            ViewBag.Payref = PayRef;
            ViewBag.Rescode = ResCode;
            ViewBag.Resdes = ResDesc;
            ViewBag.RegNo = fee.StudentReg;
            ViewBag.Name = fee.StudentName;
            return View();
        }

        //return url site for bank payment
        public ActionResult ServiceUrl(string OPR, string Cust, string ResDesc, string Amount)
        {


            //updating transaction statues
            Fee fee = db.Fees.FirstOrDefault(t => t.TransactionRef == Cust);
            fee.PaymentStatues = ResDesc;
            fee.OPR = OPR;
            db.Entry(fee).State = EntityState.Modified;
            db.SaveChanges();


            ViewBag.OPR = OPR;
            ViewBag.Cust = fee.StudentName + "(" + fee.StudentReg + ")";
            ViewBag.Amount = fee.amount.ToString("###,###.00");
            ViewBag.Resdes = ResDesc;
            return View();
        }

        public ActionResult Verify(int? id)
        {
            Fee fee = db.Fees.Find(id);
            if (fee.PaymentType == "Master/Verve/Visa Card")
            {
                var TransactionType = "2";
                ViewBag.TransactionType = TransactionType;
                var MerchantNumber = "5gs75a1sh8114a.c";
                ViewBag.MerchantNumber = MerchantNumber;
                //var SiteRedirectURL = "http://localhost:6320/Fees/ReturnUrl";
                var SiteRedirectURL = "http://smis.staloysiousschoolsedu.ng//fees/ReturnUrl";
                ViewBag.SiteRedirectURL = SiteRedirectURL;
                string hashstring = MerchantNumber + fee.TransactionRef + TransactionType + fee.amount.ToString("###") + SiteRedirectURL;
                string Transaction_Hash = SHA512(hashstring);

                string url = "https://developer.osoftpay.net/api/TestPublicPayments?TransactionNumber=" + fee.TransactionRef + "&Amount=" + fee.amount.ToString("###"); // + "&Hash=" + Transaction_Hash;

                using (HttpClient client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.DefaultRequestHeaders.Add("Hash", Transaction_Hash);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var json = content.ReadAsStringAsync().Result;
                            OSOFTRESPONSE result = JsonConvert.DeserializeObject<OSOFTRESPONSE>(json);

                            if (result != null)
                            {
                                //if (result.ResponseCode == "00")
                                //{

                                string Message = "PAYMENT STATUES REPORT" + Environment.NewLine + Environment.NewLine + "Transaction Number: " + result.TransactionReference + Environment.NewLine + "Pay Ref: " +
                                    result.PayRef + Environment.NewLine + "Payment Statues: " + result.ResponseDescription;


                                fee.PaymentStatues = result.ResponseDescription;
                                db.Entry(fee).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("Index", new { Message, Ref = fee.TransactionRef, SearchFrom = "", SearchTo = "", Statues = "", Phone = "" });
                                //}
                                //else
                                //{
                                //    string Message = "Transaction is pending, Please click on the proceed to payment to try again. Thank You";
                                //    return RedirectToAction("Create", new { Message, Ref = fee.TransactionRef, PayType = fee.PaymentType });
                                //}
                            }
                        }
                    }
                }
            }
            else if (fee.PaymentType == "Bank branch deposit/Quickteller/Phone USSD")
            {

                using (HttpClient client = new HttpClient())
                {

                    var url = "https://developer.osoftpay.net/api/TestBranchPayments?OPR=" + fee.OPR + "&Cust=" + fee.TransactionRef + "&Amount=" + fee.amount.ToString("###");
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (HttpResponseMessage response = client.GetAsync(url).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            var json = content.ReadAsStringAsync().Result;
                            APIBRANCHRESPONSE result = JsonConvert.DeserializeObject<APIBRANCHRESPONSE>
                                (json);

                            if (result != null)
                            {
                                //if (result.ResDesc.Contains("Successful"))
                                //{

                                string Message = "PAYMENT STATUES REPORT" + Environment.NewLine + Environment.NewLine + "OPR: " + result.OPR + Environment.NewLine + "Payment Statues: " + result.ResDesc;


                                fee.PaymentStatues = result.ResDesc;
                                db.Entry(fee).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("Index", new { Message, Ref = fee.TransactionRef, SearchFrom = "", SearchTo = "", Statues = "", Phone = "" });
                                //}
                                //else
                                //{
                                //    string Message = "Transaction is pending, Please click on the proceed to payment to try again. Thank You";
                                //    return RedirectToAction("Create", new { Message, Ref = fee.TransactionRef, PayType = fee.PaymentType });
                                //}
                            }
                        }
                    }
                }
            }
            string Messages = "Unable to verify payment statues at this time. Thank You.";
            return RedirectToAction("Index", new { Message = Messages, Ref = fee.TransactionRef, SearchFrom = "", SearchTo = "", Statues = "", Phone = "" });
        }



        //get for openpayment
        public ActionResult PayBill(string Message)
        {
            //CHECKING Logo
            SchoolLogo logo = db.SchoolLogoes.FirstOrDefault();
            if (logo != null)
            {
                ViewBag.Logo = logo.logo;
            }

            ViewBag.Message = Message;
            ViewBag.AccTerm = new SelectList(db.terms.OrderBy(t => t.Id), "TermName", "TermName");
            ViewBag.AccSession = new SelectList(db.Sessions.OrderBy(t => t.Id), "AcademicYear", "AcademicYear");
            return View();
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
