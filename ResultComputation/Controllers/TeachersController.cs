using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LightWay.Models;
using System.Web.Security;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using PagedList;

namespace LightWay.Controllers
{
    public class TeachersController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public TeachersController()
        {

        }

        public TeachersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            //SignInManager = signInManager;
            RoleManager = roleManager;
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }



        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        // GET: Teachers
        [Authorize(Roles = "Admin, Teacher, Accountant, Principal,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;

            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            //var ivadostaff = db.IvadoStaff.Include(i => i.Branch).Include(i => i.Position);

            var teacher = from r in db.Teachers
                          select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                teacher = teacher.Where(r => r.StaffNumber.Contains(searchString) ||
                                      r.FullName.Contains(searchString) ||
                                      r.PhoneNumber.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    teacher = teacher.OrderByDescending(r => r.StaffNumber);
                    break;
                //    //case "Date":
                //    //    students = students.OrderBy(s => s.EnrollmentDate);
                //    //    break;
                //    //case "date_desc":
                //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    //    break;
                default:  // Name ascending 
                    teacher = teacher.OrderBy(r => r.StaffNumber);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                //return View(db.Teachers.ToList().Where(c => c.StaffNumber == searchString));
                return View(db.Teachers.OrderByDescending(i => i.StaffNumber == searchString).Where(T => T.PhoneNumber == searchString || T.StaffNumber == searchString || T.FullName == searchString).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                ViewBag.Nodetails = "The Teacher with this Detail Does not Exist, ensure the right search string is entered";
                return View(db.Teachers.OrderBy(i => i.StaffNumber).ToPagedList(pageNumber, pageSize));
                //return View(db.Students.ToList());
            }

        }

        // GET: Teachers/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teachers teachers = db.Teachers.Find(id);
            if (teachers == null)
            {
                return HttpNotFound();
            }
            return View(teachers);
        }

        // GET: Teachers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.Name = new SelectList(db.Roles.Where(t=>t.Name!="Parent").ToList(), "Name", "Name");

            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,StaffNumber,UserName,FullName,Gender,ResidentialAddress,PhoneNumber,EmailAddress,Passport,Name,DOA,BasicSalary,RegisteredBy,DateRecorded")] Teachers teachers, HttpPostedFileBase file)
        {
            string filename = "";

            byte[] bytes;

            int BytestoRead;

            int numBytesRead;
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            //Searching Teacher
            Teachers TeacherNum = db.Teachers.FirstOrDefault(t => t.StaffNumber == teachers.StaffNumber);
            Teachers teacherEmail = db.Teachers.FirstOrDefault(t => t.EmailAddress == teachers.EmailAddress);
            if (TeacherNum == null && teacherEmail == null)
            {

                //default password
                string password = "1234567";

                //saving other Records
                teachers.RegisteredBy = User.Identity.Name;
                teachers.DateRecorded = DateTime.Now;


                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        //Save Image to DB as Byte
                        filename = Path.GetFileName(file.FileName);

                        bytes = new byte[file.ContentLength];

                        BytestoRead = (int)file.ContentLength;

                        numBytesRead = 0;

                        while (BytestoRead > 0)
                        {

                            int n = file.InputStream.Read(bytes, numBytesRead, BytestoRead);

                            if (n == 0) break;

                            numBytesRead += n;

                            BytestoRead -= n;

                        }

                        teachers.Passport = bytes;

                    }
                    db.Teachers.Add(teachers);
                    db.SaveChanges();

                    //get teacher Id
                    int id = teachers.Id;

                    //Creating Account
                    var user = new ApplicationUser { UserName = teachers.UserName, Email = teachers.EmailAddress };
                    var result = await UserManager.CreateAsync(user, password);

                    //Assign Role to user Here 
                    await this.UserManager.AddToRoleAsync(user.Id, teachers.Name);

                    ////sending Email to Agents
                    //string FromMail = "info@baptisthighschoolmarke.com";
                    //string MailTopic = "Nofication";
                    //string SMTP = "smtp.baptisthighschoolmarke.com";
                    //string MailPassword = "JFSNMCq0";


                    //System.Net.Mail.MailMessage mailObj = new System.Net.Mail.MailMessage(
                    //FromMail, teachers.EmailAddress, MailTopic, "Hello" + Environment.NewLine + Environment.NewLine
                    //+ "This is your User Details for the School Result computation System" + Environment.NewLine + Environment.NewLine +
                    //"UserName: " + teachers.EmailAddress + Environment.NewLine + Environment.NewLine + "Password: " + password + Environment.NewLine + Environment.NewLine
                    //+ "Please to login click this link:" + "http://www.baptisthighschoolmarke" + "please on Login, you are advice to change it.");

                    //mailObj.CC.Add(FromMail);

                    //SmtpClient SMTPServer = new SmtpClient(SMTP);

                    //SMTPServer.Credentials = new System.Net.NetworkCredential(FromMail, MailPassword);
                    //SMTPServer.Send(mailObj);


                    return RedirectToAction("Create", "TeacherSubjects", new { id });
                }
            }

            else
            {
                ViewBag.ErrorInStudtNum = "Please Ensure that the Staff Number And Staff Email as not been entered before";
                ViewBag.Name = new SelectList(db.Roles.ToList(), "Name", "Name");
            }
            return View(teachers);
        }

        // GET: Teachers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teachers teachers = db.Teachers.Find(id);
            if (teachers == null)
            {
                return HttpNotFound();
            }
            ViewBag.Name = new SelectList(db.Roles.ToList(), "Name", "Name");
            ViewBag.Passport = teachers.Passport;
            ViewBag.DOA = teachers.DOA;
            return View(teachers);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,StaffNumber,UserName,FullName,Gender,ResidentialAddress,PhoneNumber,EmailAddress,Passport,Name,DOA,BasicSalary,RegisteredBy,DateRecorded")] Teachers teachers, HttpPostedFileBase file)
        {
            //getting other student details
            Teachers std = db.Teachers.FirstOrDefault(t => t.StaffNumber == teachers.StaffNumber);

            //if passport is uploaded during edit
            string filename = "";

            byte[] bytes;

            int BytestoRead;

            int numBytesRead;

            if (file != null)
            {
                if (file != null && file.ContentLength > 0)
                {
                    //Save Image to DB as Byte
                    filename = Path.GetFileName(file.FileName);

                    bytes = new byte[file.ContentLength];

                    BytestoRead = (int)file.ContentLength;

                    numBytesRead = 0;

                    while (BytestoRead > 0)
                    {

                        int n = file.InputStream.Read(bytes, numBytesRead, BytestoRead);

                        if (n == 0) break;

                        numBytesRead += n;

                        BytestoRead -= n;

                    }

                    teachers.Passport = bytes;

                }
            }
            else
            {
                teachers.Passport = std.Passport;
            }

            teachers.RegisteredBy = std.RegisteredBy;
            teachers.DateRecorded = std.DateRecorded;
            teachers.Name = std.Name;

            //Saving Records to Db
            db.Entry(std).CurrentValues.SetValues(teachers);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Teachers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, string Email)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teachers teachers = db.Teachers.Find(id);
            if (teachers == null)
            {
                return HttpNotFound();
            }
            return View(teachers);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id, string Email)
        {
            Teachers teachers = db.Teachers.Find(id);

            //deleting all subject related to the teacher
            var subjects = db.TeacherSubjects.ToList().Where(t => t.TeacherNum == teachers.StaffNumber);
            foreach (var item in subjects)
            {
                db.TeacherSubjects.Remove(item);
                db.SaveChanges();
            }

            db.Teachers.Remove(teachers);
            db.SaveChanges();


            //deleting User and their respective Roles
            if (ModelState.IsValid)
            {
                if (Email == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByEmailAsync(Email);
                var logins = user.Logins;

                foreach (var login in logins.ToList())
                {
                    await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
                }

                var rolesForUser = await UserManager.GetRolesAsync(user.Id);

                if (rolesForUser.Count() > 0)
                {
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                    }
                }

                //db.Users.Remove(user);
                //db.SaveChanges();

                var r = await UserManager.DeleteAsync(user);
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }


        }

        //Assigning Subject to Teacher
        //[Authorize(Roles = "Admin")]
        //public ActionResult TeacherSubject(int id)
        //{
        //    return RedirectToAction("Create", "TeacherSubjects", new { id });
        //}


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();


                if (UserManager != null)
                {
                    UserManager.Dispose();
                    UserManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
