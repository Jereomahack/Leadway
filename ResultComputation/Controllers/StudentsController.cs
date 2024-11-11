using LightWay.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class StudentsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Index(int? Id, string sortOrder, string classFilter, string Class, int? page)
        {

            //getting user from Teacher Db
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            //getting teacher class
            TeacherSubject teacherclass = db.TeacherSubjects.FirstOrDefault(t => t.TeacherNum == user.StaffNumber);

            //getting the search class
            int classid = Convert.ToInt32(Class);
            TeacherSubject dbClass = db.TeacherSubjects.Find(classid);

            //Uploading Teacher Subjects
            //ViewBag.Subject = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber), "Id", "Subject");
            ViewBag.Class = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber), "Id", "Class");


            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (Class != null)
            {
                page = 1;
                classFilter = dbClass.Class;
                Class = classFilter;

            }
            else
            {
                Class = classFilter;
            }

            //ViewBag.ClassFilter = Class;

            //ViewBag.Subject = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber), "Id", "Subject");
            ViewBag.Class = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber), "Id", "Class");


            var student = from r in db.Students
                          select r;
            if (!String.IsNullOrEmpty(Class))
            {
                student = student.Where(r => r.Class.Contains(Class));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    student = student.OrderByDescending(r => r.Surname);
                    break;
                //    //case "Date":
                //    //    students = students.OrderBy(s => s.EnrollmentDate);
                //    //    break;
                //    //case "date_desc":
                //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    //    break;
                default:  // Name ascending 
                    student = student.OrderBy(r => r.Surname);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);

            if (Class != null)
            {

                return View(db.Students.OrderBy(i => i.Surname).Where(t => t.Transistion == "No" && t.Class == dbClass.Class).ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(db.Students.OrderBy(i => i.Surname).Where(t => t.Transistion == "No" && t.Class == teacherclass.Class).ToPagedList(pageNumber, pageSize));
            }


        }


        public ActionResult IndexAdmission(string Search)
        {
            if (Search != null)
            {
                if (Search != "")
                {

                    var adm = from d in db.admissions
                              where d.FormNo.Contains(Search) || d.Firstname.Contains(Search) || d.Fgsmnumber.Contains(Search)
                              select d;
                    return View(adm.ToList());
                }
            }


            return View(db.admissions.Where(t => t.Status != "Admitted").OrderByDescending(t => t.DateTime).Take(100).ToList());
        }


        //index Admin
        [Authorize(Roles = "Admin,Accountant,Principal")]
        public ActionResult IndexAdmin(int? Id, int? SubjectID, string sortOrder, string currentFilter, string searchString, int? page, string Adnum)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else if (Adnum != null)
            {
                ViewBag.Number = Adnum;
            }
            else
            {
                searchString = currentFilter;

            }

            ViewBag.CurrentFilter = searchString;

            //var ivadostaff = db.IvadoStaff.Include(i => i.Branch).Include(i => i.Position);

            var student = from r in db.Students
                          select r;
            if (!String.IsNullOrEmpty(searchString))
            {
                student = student.Where(r => r.StudentNumber.Contains(searchString)
                                       || r.Class.Contains(searchString)
                                       || r.Surname.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    student = student.OrderByDescending(r => r.StudentNumber);
                    break;
                //    //case "Date":
                //    //    students = students.OrderBy(s => s.EnrollmentDate);
                //    //    break;
                //    //case "date_desc":
                //    //    students = students.OrderByDescending(s => s.EnrollmentDate);
                //    //    break;
                default:  // Name ascending 
                    student = student.OrderBy(r => r.StudentNumber);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);

            if (searchString != null)
            {
                return View(db.Students.OrderByDescending(i => i.StudentNumber == searchString || i.Class == searchString || i.Surname == searchString).Where(t => /*t.Transistion == "No" &&*/ t.Class != "Graduated").ToPagedList(pageNumber, pageSize));

            }
            else
            {
                return View(db.Students.OrderBy(i => i.Class).Where(i => /*i.Transistion == "No" &&*/ i.Class != "Graduated").ToPagedList(pageNumber, pageSize));
            }

        }


        // GET: Students/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewBag.Class = new SelectList(db.ClassLists.OrderBy(t => t.ClassName), "ClassName", "ClassName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,StudentNumber,AlternativePhoneNumber,Surname,OtherName,Gender,DOB,Class,GuardianName,GuardianAddress,EmailAddress,PhoneNumber,Passport,Category,CheckResult,Transistion,Weight,Height,GeneralSactisfaaction,Session")] Student student, HttpPostedFileBase file, string ClassListFiter)
        {

            ViewBag.Class = new SelectList(db.ClassLists.OrderBy(t => t.ClassName), "ClassName", "ClassName");
            string filename = "";

            byte[] bytes;

            int BytestoRead;

            int numBytesRead;
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            //searching Student Number
            Student stdntNum = db.Students.FirstOrDefault(s => s.StudentNumber == student.StudentNumber);

            if (stdntNum == null)
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

                    student.Passport = bytes;

                }

                //Setting Transistion
                student.Transistion = "No";

                if (student.StudentNumber == null)
                {
                    GlobalSettings Addm = db.GlobalSettings.FirstOrDefault(u => u.Name == "Admission No");
                    int NewAdnum = Convert.ToInt32(Addm.Value) + 1;
                    if (NewAdnum.ToString().Length <= 3)
                    {
                        student.StudentNumber = NewAdnum.ToString("D3");
                    }
                    else
                    {
                        student.StudentNumber = NewAdnum.ToString();
                    }



                    //updating Admission Number in globalsetting

                    Addm.Value = Convert.ToString(NewAdnum);

                    db.Entry(Addm).State = EntityState.Modified;
                    db.SaveChanges();
                }


                //get school name
                GlobalSettings getschool = db.GlobalSettings.FirstOrDefault(t => t.Name == "School");



                if (student.EmailAddress == null)
                {
                    student.EmailAddress = "null@gmail.com";
                }
                //parent username ans password
                if (!db.Roles.Any(r => r.Name == "Parent"))
                {
                    var store = new RoleStore<IdentityRole>(db);
                    var manager = new RoleManager<IdentityRole>(store);
                    var role = new IdentityRole { Name = "Parent" };

                    manager.Create(role);
                }

                if (!db.Users.Any(u => u.UserName == student.PhoneNumber))
                {
                    var store = new UserStore<ApplicationUser>(db);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName = student.PhoneNumber, Email = student.EmailAddress, PhoneNumber = student.PhoneNumber, RoleName = "Parent" };

                    manager.Create(user, "1234567");
                    manager.AddToRole(user.Id, "Parent");



                    if (student.PhoneNumber != null)
                    {

                        //sendind sms to parent
                        string message = "To check your child result, enter username as " + student.PhoneNumber + " and password as 1234567" + "  on http://smis.staloysiousschoolsedu.ng Thanks. Please note this is a default password";
                        string pwd = "smispwd12345";
                        string YourUsername = "OSOFTSMIS";
                        string sendto = student.PhoneNumber;
                        string senderid = getschool.Value;
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

                    }

                    if (student.EmailAddress != "null@gmail.com")
                    {
                        //sending mail 
                        string Topic = getschool.Value + " LOGIN DETAIL";
                        string email = student.EmailAddress;
                        String Message = "Hello" + Environment.NewLine + Environment.NewLine + "To check your child(ren) Result. Below is your login informations." + Environment.NewLine + Environment.NewLine +
                            "Username: " + student.PhoneNumber + Environment.NewLine + "Password: 1234567, please note that this password is default, which should be changed to your desired password. Thank You" +
                            Environment.NewLine + "click on this link to login http://smis.staloysiousschoolsedu.ng" + Environment.NewLine + "Please for other enquiry confirm with the school administrator. Thank You.";
                        general mail = new general();
                        mail.SendMail(Message, Topic, email);

                    }

                }

               


                //Saving other Records
                student.RegisteredBy = User.Identity.Name;
                student.DateRecorded = DateTime.Now;

                //Saving all rECORDS TO database
                db.Students.Add(student);
                db.SaveChanges();


                //getting student number
                string Adnum = student.StudentNumber;

                //get student id to add Subjects
                int id = student.Id;

                return RedirectToAction("IndexAdmin", new { id, Adnum });
            }
            else
            {
                ViewBag.ErrorInStudtNum = "The Student Number" + " " + student.StudentNumber + " " + " Already Exist";

            }
            ViewBag.ClassListFiter = new SelectList(db.ClassLists, "Id", "ClassName", student.Class);

            return View(student);
        }


        //get
        public ActionResult Admission(int id)
        {
            ViewBag.AId = id;
            ViewBag.Class = new SelectList(db.ClassLists.OrderBy(t => t.ClassName), "ClassName", "ClassName");
            return PartialView();
        }

        //post
        [HttpPost]
        public ActionResult Admission([Bind(Include = "Id,StudentNumber,Surname,OtherName,Gender,DOB,Class,GuardianName,GuardianAddress,EmailAddress,PhoneNumber,Passport,Category,CheckResult,Transistion,Weight,Height,GeneralSactisfaaction,Session")] Student student, string AId)
        {

            //get admission detail
            Admission admission = db.admissions.Find(Convert.ToInt32(AId));
            if (admission != null)
            {

                GlobalSettings Addm = db.GlobalSettings.FirstOrDefault(u => u.Name == "Admission No");
                int NewAdnum = Convert.ToInt32(Addm.Value) + 1;
                if (NewAdnum.ToString().Length <= 3)
                {
                    student.StudentNumber = NewAdnum.ToString("D3");
                }
                else
                {
                    student.StudentNumber = NewAdnum.ToString();
                }



                //updating Admission Number in globalsetting

                Addm.Value = Convert.ToString(NewAdnum);

                db.Entry(Addm).State = EntityState.Modified;
                db.SaveChanges();

                //creating new object of student
                Student newstudent = new Student()
                {
                    Surname = admission.Surname,
                    OtherName = admission.Firstname + " " + admission.Middlename,
                    Gender = admission.Gender,
                    EmailAddress = admission.FemailAddress,
                    Category = student.Category,
                    DateRecorded = DateTime.Now,
                    CheckResult = "Allow",
                    DOB = admission.DOB,
                    Class = student.Class,
                    GeneralSactisfaaction = student.GeneralSactisfaaction,
                    GuardianAddress = admission.Faddress,
                    GuardianName = admission.FsurnName,
                    Height = student.Height,
                    Passport = admission.Passport,
                    PhoneNumber = admission.Fgsmnumber,
                    Session = student.Session,
                    RegisteredBy = User.Identity.Name,
                    StudentNumber = student.StudentNumber,
                    Transistion = "No",
                    Weight = student.Weight
                };
                db.Students.Add(newstudent);
                db.SaveChanges();
            }


            //updating Admission
            admission.Status = "Admitted";
            db.Entry(admission).State = EntityState.Modified;
            db.SaveChanges();

            string message = admission.Surname + " " + admission.Firstname + " " + admission.Middlename + " was successfully Admitted into " + student.Class + ". Admission Number is: " + student.StudentNumber + " Thank You.";
            return Json(new { message });
        }


        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {

            ViewBag.ClassListFiter = new SelectList(db.ClassLists, "Id", "ClassName");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DOB = student.DOB.ToString("MM/dd/yyyy");
            ViewBag.Passport = student.Passport;
            TempData["Session"] = student.Session;
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,StudentNumber,AlternativePhoneNumber,Surname,OtherName,Gender,DOB,Class,GuardianName,GuardianAddress,EmailAddress,PhoneNumber,Passport,Category,CheckResult,Transistion,Weight,Height,GeneralSactisfaaction,Session")] Student student, HttpPostedFileBase file)
        {

            //getting other student details
            Student std = db.Students.FirstOrDefault(t => t.StudentNumber == student.StudentNumber);

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

                    BytestoRead = file.ContentLength;

                    numBytesRead = 0;

                    while (BytestoRead > 0)
                    {

                        int n = file.InputStream.Read(bytes, numBytesRead, BytestoRead);

                        if (n == 0) break;

                        numBytesRead += n;

                        BytestoRead -= n;

                    }

                    student.Passport = bytes;

                }
            }
            else
            {
                student.Passport = std.Passport;
            }

            student.Transistion = std.Transistion;
            student.RegisteredBy = std.RegisteredBy;
            student.DateRecorded = std.DateRecorded;
            student.Session = Convert.ToString(TempData["Session"]);
            student.Transistion = std.Transistion;

            //updating Parent code informations
            ParentCode UPDATECODE = db.ParentCodes.FirstOrDefault(r => r.PhoneNumber == std.PhoneNumber);
            if (UPDATECODE != null)
            {
                if (UPDATECODE.PhoneNumber != student.PhoneNumber)
                {
                    UPDATECODE.PhoneNumber = student.PhoneNumber;

                    db.Entry(UPDATECODE).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }


            //Saving Records to Db
            db.Entry(std).CurrentValues.SetValues(student);
            db.SaveChanges();

            return RedirectToAction("IndexAdmin");

        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }


        //Update Student Result
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult UpdateResult(int id)
        {

            return RedirectToAction("IndexUpdate", "scores", new { id });
        }


        //Create New Result
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult NewResult(int id)
        {

            return RedirectToAction("Create", "scores", new { id });
        }

        //Assigning Subjects to Student
        [Authorize(Roles = "Admin")]
        public ActionResult StudentSubject(int id)
        {

            return RedirectToAction("Create", "StudentSubjects", new { id });
        }

        //list Teacher Subject
        public ActionResult TeacherSubjects()
        {
            //getting user from Teacher Db
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);
            return View(db.TeacherSubjects.ToList().Where(y => y.EmailAddress == user.EmailAddress));
        }

        //List Teacher Students
        public ActionResult TeacherStudent()
        {
            //getting user from Teacher Db
            Teachers user = db.Teachers.FirstOrDefault(u => u.EmailAddress == User.Identity.Name);

            //Searching for class and Subject

            if (User.IsInRole("Form Master"))
            {
                ClassList Tclass = db.ClassLists.FirstOrDefault(t => t.FormMaster == user.FullName);
                return View(db.Students.ToList().Where(s => s.Class == Tclass.ClassName));

            }

            TeacherSubject teacher = db.TeacherSubjects.FirstOrDefault(c => c.TeacherNum == user.StaffNumber);
            return View(db.Students.ToList().Where(s => s.Class == teacher.Class));
        }


        //student Transition
        public ActionResult StudentTransistions(int id)
        {
            Student student = db.Students.Find(id);

            if (student.Transistion == "No")
            {
                student.Transistion = "Yes";
            }
            else
            {
                student.Transistion = "No";
            }

            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("IndexAdmin");
        }


        //Student Promotion
        [Authorize(Roles = "Admin")]
        public ActionResult Promotion(int id)
        {

            Student student = db.Students.Find(id);
            if (student.Class == "JS 1")
            {
                student.Class = "JS 2";
            }
            else if (student.Class == "JS 2")
            {
                student.Class = "JS 3";
            }
            else if (student.Class == "JS 3")
            {
                student.Class = "SS 1";
                student.Category = "Senior";

            }
            else if (student.Class == "SS 1")
            {
                student.Class = "SS 2";
            }
            else if (student.Class == "SS 2")
            {
                student.Class = "SS 3";
            }
            else if (student.Class == "SS 3")
            {
                student.Class = "Graduated";
            }
            else if (student.Class == "PRI 1")
            {
                student.Class = "PRI 2";
            }
            else if (student.Class == "PRI 2")
            {
                student.Class = "PRI 3";
            }
            else if (student.Class == "PRI 3")
            {
                student.Class = "PRI 4";
            }
            else if (student.Class == "PRI 4")
            {
                student.Class = "PRI 5";
            }
            else if (student.Class == "PRI 5")
            {
                student.Class = "PRI 6";
            }
            else if (student.Class == "PRI 6")
            {

                //Saving Student Old Numbers
                StudentOldNumbers odlnum = new StudentOldNumbers()
                {
                    StudentName = student.Surname + " " + student.OtherName,
                    StudentNumber = student.StudentNumber

                };
                db.OldAdmissionNumbers.Add(odlnum);
                db.SaveChanges();

                GlobalSettings SecAd = db.GlobalSettings.FirstOrDefault(u => u.Name == "Admission No");
                int NewAdnum = Convert.ToInt32(SecAd.Value) + 1;
                student.StudentNumber = "LWA " + DateTime.Now.ToString("yy") + "/" + DateTime.Now.AddYears(1).ToString("yy") + "-" + Convert.ToString(NewAdnum);
                //student.StudentNumber = "LWA " + Convert.ToString(NewAdnum) + "/" + DateTime.Now.ToString("yy");
                student.StudentNumber = Convert.ToString(NewAdnum);


                SecAd.Value = Convert.ToString(NewAdnum);

                db.Entry(SecAd).State = EntityState.Modified;
                db.SaveChanges();

                student.Class = "JS 1";
                student.Category = "Junior";
                student.Session = "Secondary";
            }
            else if (student.Class == "NUR 1")
            {
                student.Class = "NUR 2";
            }
            else if (student.Class == "NUR 2")
            {
                student.Class = "NUR 3";
            }
            else if (student.Class == "NUR 3")
            {

                student.Class = "PRI 1";
                student.Category = "Primary";
                student.Session = "Primary";
            }
            else if (student.Class == "PRE-NUR")
            {
                student.Class = "NUR 1";
                student.Category = "Nursery";
            }

            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
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
