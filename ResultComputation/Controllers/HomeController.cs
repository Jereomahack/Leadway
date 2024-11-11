using LightWay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class HomeController : BaseController
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //Daboard
        public ActionResult DashBoard()
        {
            //getting the user
            Teachers user = db.Teachers.FirstOrDefault(u => u.UserName == User.Identity.Name || u.EmailAddress == User.Identity.Name);


            if (User.IsInRole("Class Teacher PRE-NUR") || User.IsInRole("Class Teacher NUR 1") || User.IsInRole("Class Teacher NUR 2") || User.IsInRole("Class Teacher NUR 3") || User.IsInRole("Class Teacher PRI 1") || User.IsInRole("Class Teacher PRI 2") || User.IsInRole("Class Teacher PRI 3") || User.IsInRole("Class Teacher PRI 4") || User.IsInRole("Class Teacher PRI 5") || User.IsInRole("Class Teacher PRI 6"))
            {
                //getting User Details
                TeacherSubject TsUB = db.TeacherSubjects.FirstOrDefault(c => c.TeacherNum == user.StaffNumber);

                if (TsUB != null)
                {
                    foreach (var item in db.TeacherSubjects.Where(t => t.TeacherNum == user.StaffNumber).ToList())
                    {
                        ViewBag.Tstudent = db.Students.Where(r => r.Class != "Graduated").Count(t => t.Class == item.Class);
                    }
                }
                else
                {
                    ViewBag.Tstudent = 0;
                }

            }
            if (User.IsInRole("Principal") || User.IsInRole("Accountant") || User.IsInRole("Cashier"))
            {
                //getting Term And Academic Year from Globalsettings
                GlobalSettings term = db.GlobalSettings.FirstOrDefault(g => g.Name == "Term");
                GlobalSettings Year = db.GlobalSettings.FirstOrDefault(g => g.Name == "Session");
                ViewBag.Term = term.Value;
                ViewBag.Session = Year.Value;
                //calculating Total Amount Generated for Term and Session in GlobalSettings Table
                var TermAmount = from a in db.Fees
                                 where a.AccTerm == term.Value && a.AccSession == Year.Value && a.PaymentStatues == "Approved Successful"
                                 select a;

                if (TermAmount.Any())
                {

                    ViewBag.TotalAmount = TermAmount.Sum(t => t.amount).ToString("#,##0.00");
                }


                //calculating Total Amount Generated for present Session
                var SessAmount = from a in db.Fees
                                 where a.AccSession == Year.Value && a.PaymentStatues == "Approved Successful"
                                 select a;

                if (SessAmount.Any())
                {

                    ViewBag.SessAmount = SessAmount.Sum(t => t.amount).ToString("N");
                }



                //calculating Total Expenditure Generated for Term and Session in GlobalSettings Table
                var Exp = from a in db.Expenditures
                          where a.Term == term.Value && a.Session == Year.Value && a.Statues == "Approved"
                          select a;

                if (Exp.Any())
                {
                    ViewBag.TotalExpenditures = Exp.Sum(t => t.AmountRequested).ToString("N");
                }



                //Calculating Balance for the Term
                //var Tbal = tamt - Texp;
                //ViewBag.TotalBal = Tbal.ToString("#,##0.00");
                ViewBag.TotalBal = 0;


            }
            if (User.IsInRole("Class Teacher PRE-NUR") || User.IsInRole("Teacher") || User.IsInRole("Class Teacher NUR 1") || User.IsInRole("Class Teacher NUR 2") || User.IsInRole("Class Teacher NUR 3") || User.IsInRole("Class Teacher PRI 1") || User.IsInRole("Class Teacher PRI 2") || User.IsInRole("Class Teacher PRI 3") || User.IsInRole("Class Teacher PRI 4") || User.IsInRole("Class Teacher PRI 5") || User.IsInRole("Class Teacher PRI 6") || User.IsInRole("Accountant") || User.IsInRole("Cashier") || User.IsInRole("Store Keeper"))
            {
                //getting Teacher FullName and Teacher Role to dashboard View
                ViewBag.UserName = user.FullName;
                ViewBag.Role = user.Name;
            }
            else if (User.IsInRole("SuperUser"))
            {
                ViewBag.UserName = "Super User";
                ViewBag.Role = "Administrator";
            }
            else if (User.IsInRole("Admin"))
            {
                ViewBag.UserName = "Administrator";
                ViewBag.Role = "Administrator";
            }
            else
            {
                ViewBag.UserName = User.Identity.Name;
            }
            //substracting graduated students from present students

            //searching database for graduated students
            int countGraduated = db.Students.Count(se => se.Class == "Graduated");

            //searching students on transistion
            int Stdtransist = db.Students.Count(r => r.Transistion == "Yes");

            //total of Graduated and Transisted Students
            int TranGrad = countGraduated + Stdtransist;

            //counting total students from database
            var TStudent = db.Students.SqlQuery(" SELECT * FROM dbo.Students ").Count();

            //subtracting graduated students from total students
            var realstudents = TStudent - TranGrad;


            //Getting total numbers of student, teacher, class, subjects from table
            ViewBag.Student = realstudents;
            ViewBag.Teachers = db.Teachers.SqlQuery(" SELECT * FROM dbo.Teachers ").Count();
            ViewBag.Class = db.ClassLists.SqlQuery(" SELECT * FROM dbo.ClassLists ").Count();
            ViewBag.Subjects = db.SchoolSubjectss.SqlQuery(" SELECT * FROM dbo.SchoolSubjects ").Count();

            //getting total Notification that is not read
            //if (User.IsInRole("Admin"))
            //{
            //    var count1 = db.Emailings.Count(c => c.Reciever == "Admin" && c.Statues == "Not Read");
            //    ViewBag.Notification = count1;

            //}
            //else
            //{
            //    if(!User.IsInRole("Parent"))
            //    {
            //        var count = db.Emailings.Where(c => c.Reciever == user.Name && c.Statues == "Not Read").Count();
            //        ViewBag.Notification = count;
            //    }

            //}

            return View();
        }


    }
}