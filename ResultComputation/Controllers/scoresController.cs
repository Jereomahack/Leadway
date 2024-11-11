using LightWay.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class scoresController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: scores
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult IndexUpdate(string sortOrder, string SubjectFilter, string Subject, int? page, string Class, string Category, string Message)
        {
            TempData["MSG"] = Message;
            //getting user from Teacher Db
            Teachers user = db.Teachers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            TempData["Category"] = Category;
            ViewBag.Category = Category;

            //get distict class
            var listclass = from d in db.TeacherSubjects
                            where d.TeacherNum == user.StaffNumber
                            select d.Class;

            if (listclass.Any())
            {
                var dst = listclass.Distinct();
                ViewBag.Class = new SelectList(dst).ToList();
            }


            ViewBag.Subject = new SelectList("", "", "");

            //ViewBag.Subject = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber), "Subject", "Subject");
            //ViewBag.Class = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber), "Class", "Class");

            //getting records from global settings
            GlobalSettings global = db.GlobalSettings.FirstOrDefault(g => g.Name == "Term");
            GlobalSettings session = db.GlobalSettings.FirstOrDefault(g => g.Name == "Session");

            if (Subject != null && Class != null)
            {
                return View(db.scores.Where(t => t.Class == Class && t.Subject == Subject && t.Term == global.Value && t.AcademicYear == session.Value).ToList());
            }
            else
            {
                return View(db.scores.ToList().Take(0));
            }

        }


        //Index for all Score
        [Authorize(Roles = "Admin, Teacher, Class Teacher PRE-NUR, Class Teacher NUR 1, Class Teacher NUR 2, Class Teacher NUR 3, Class Teacher PRI 1, Class Teacher PRI 2, Class Teacher PRI 3, Class Teacher PRI 4, Class Teacher PRI 5, Class Teacher PRI 6")]
        public ActionResult IndexAll(string sortOrder, string StudtNumFilter, string StudtNum, string AccademicYearFilter, string AccademicYear, string TermFilter, string Terms, int? page)
        {

            //CHECKING Logo
            SchoolLogo logo = db.SchoolLogoes.FirstOrDefault();
            if (logo != null)
            {
                ViewBag.Logo = logo.logo;
            }

            ViewBag.ClassList = new SelectList(db.ClassLists, "ClassName", "ClassName");
            ViewBag.StudtNum = new SelectList("", "", "");
            //getting Student
            Student studt = db.Students.FirstOrDefault(s => s.StudentNumber == StudtNum.Trim());

            //getting Term 
            int termID = Convert.ToInt32(Terms);
            Term term = db.terms.Find(termID);

            //getting Session
            int sessionID = Convert.ToInt32(AccademicYear);
            Session session = db.Sessions.Find(sessionID);

            //getting dropdown
            ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (StudtNum != null && AccademicYear != null && Terms != null)
            {
                page = 1;
            }

            else
            {
                StudtNum = StudtNumFilter;
                AccademicYear = AccademicYearFilter;
                Terms = TermFilter;
            }

            ViewBag.StudtNumFilter = StudtNum;
            ViewBag.AccademicYearFilter = AccademicYear;
            ViewBag.TermFilter = Terms;

            //var ivadostaff = db.IvadoStaff.Include(i => i.Branch).Include(i => i.Position);

            ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");


            var student = from r in db.scores
                          select r;
            if (!String.IsNullOrEmpty(StudtNum))
            {
                student = student.Where(r => r.StudentNumber.Contains(StudtNum.Trim())
                                       && r.AcademicYear.Contains(AccademicYear)
                                        && r.Term.Contains(Terms));
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



            if (StudtNum == null && AccademicYear == null && Terms == null)
            {
                ViewBag.AcademicYear = "";
                ViewBag.Term = "";
                ViewBag.AdmissionNo = "";
                ViewBag.Surname = "";
                ViewBag.OtherName = "";
                ViewBag.Class = "";
                ViewBag.ObtainableScore = "";
                ViewBag.OverAllGrades = "";
                ViewBag.OverallRating = "";
                //ViewBag.Passport = 0x00;
                ViewBag.EndTerm = "";
                ViewBag.NextTerm = "";
                ViewBag.FessOwed = "";
                ViewBag.FessPaid = "";
                ViewBag.Tclass = "";

                ViewBag.NoDetails = "No Details Found";
                return View();

            }

            else if (StudtNum != null && AccademicYear != null && Terms != null)
            {

                //findind search from Database
                score scoresearch = db.scores.FirstOrDefault(s => s.AcademicYear == session.AcademicYear && s.StudentNumber == StudtNum.Trim() && s.Term == term.TermName);
                if (scoresearch != null)
                {
                    //Geting View Bag Value to display on the IndexAll
                    ViewBag.AdmissionNo = studt.StudentNumber;
                    ViewBag.Surname = studt.Surname;
                    ViewBag.OtherName = studt.OtherName;
                    ViewBag.Class = studt.Class;

                    //Accademic Year
                    ViewBag.AcademicYear = session.AcademicYear;

                    //Term
                    ViewBag.Term = term.TermName;

                    ViewBag.Category = studt.Category;

                    //getting Ending Term from GlobalSettings
                    GlobalSettings EndTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "EndTerm");
                    ViewBag.EndTerm = EndTerm.Value;

                    //getting Next term from GlobalSettings
                    GlobalSettings NextTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "NextTerm");
                    ViewBag.NextTerm = NextTerm.Value;

                    //getting student passport
                    ViewBag.Passport = studt.Passport;

                    //getting Total number in Class
                    var count = db.Students.Count(s => s.Class == studt.Class && s.Transistion == "No");
                    ViewBag.Tclass = count;

                    //getting  Fee Owed
                    //var c = from p in db.payments
                    //        where p.StudentNum == StudtNum
                    //        select p;
                    //if (c != null)
                    //{
                    //    ViewBag.FeesOwed = c.Sum(t => t.Balance);
                    //}
                    //else
                    //{
                    //    ViewBag.FeesOwed = null;
                    //}

                    ViewBag.FeesOwed = "";

                    //getting Fee Paid
                    Payment payment = db.payments.FirstOrDefault(p => p.StudentNum == StudtNum && p.Session == session.AcademicYear && p.Term == term.TermName);

                    //if (payment != null)
                    //{
                    //    ViewBag.FeesPaid = payment.Amount;
                    //}
                    //else
                    //{
                    //    ViewBag.FeesPaid = null;
                    //}
                    ViewBag.FeesPaid = "";

                    if (studt.Category == "Primary" || studt.Category == "Senior" || studt.Category == "Junior")
                    {
                        //getting total Cognitive, Affective, Psychomoto, Exam, Scores 
                        ViewBag.TotalCognitive = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.FirstCA).ToString("0");
                        ViewBag.TotalAffective = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.SecondCA).ToString("0");
                        ViewBag.TotalPsychomoto = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.Psychomoto).ToString("0");
                        ViewBag.TotalExam = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.Exam);
                        ViewBag.TotalScore = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.Total).ToString("0");
                    }
                    else if (studt.Category == "Nursery" || studt.Category == "Pre-Nursery")
                    {
                        ViewBag.Tobtainable = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.ObtainableScore).ToString("0");
                        ViewBag.Tobtained = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.Exam).ToString("0");
                    }



                    //getting Overall Score for Student
                    score studnt1 = db.scores.FirstOrDefault(u => u.StudentNumber == StudtNum);

                    var x = from d in db.scores
                            where d.StudentNumber == StudtNum && d.AcademicYear == session.AcademicYear && d.Term == term.TermName
                            select d;

                    decimal OverallScores = x.Sum(y => y.Total);
                    var count2 = x.Count();
                    decimal Gradess = OverallScores / count2;

                    //Getting Obtainable Scores
                    ViewBag.ObtainableScore = count2 * 100;

                    if (Gradess >= 80)
                    {
                        ViewBag.OverAllGrades = "A";
                        ViewBag.OverallRating = "EXCELLENT";
                    }
                    else if (Gradess >= 70)
                    {
                        ViewBag.OverAllGrades = "B";
                        ViewBag.OverallRating = "GOOD";
                    }
                    else if (Gradess >= 60)
                    {
                        ViewBag.OverAllGrades = "C";
                        ViewBag.OverallRating = "CREDIT";
                    }
                    else if (Gradess >= 50)
                    {
                        ViewBag.OverAllGrades = "D";
                        ViewBag.OverallRating = "PASS";
                    }
                    else if (Gradess >= 40)
                    {
                        ViewBag.OverAllGrades = "E";
                        ViewBag.OverallRating = "PASS";
                    }
                    else if (Gradess < 40)
                    {
                        ViewBag.OverAllGrades = "F";
                        ViewBag.OverallRating = "FAIL";
                    }


                    if (term.TermName == "THIRD")
                    {

                        //counting total numbers of subject offerred Per-Term
                        var count1st = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "FIRST" && t.AcademicYear == session.AcademicYear).Count();
                        var count2nd = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "SECOND" && t.AcademicYear == session.AcademicYear).Count();
                        var count3rd = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "THIRD" && t.AcademicYear == session.AcademicYear).Count();

                        //calculating cumulative scores for third term
                        var TFirst = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "FIRST" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);
                        var TSecond = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "SECOND" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);
                        var TThird = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "THIRD" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);

                        //calculating overall score for each Term
                        var First = TFirst / count1st;
                        var Second = TSecond / count2nd;
                        var Third = TThird / count3rd;

                        //total score for first, Second, Third Term of a Session
                        ViewBag.FirstTerm = TFirst.ToString("0");
                        ViewBag.SecondTerm = TSecond.ToString("0");
                        ViewBag.ThirdTerm = TThird.ToString("0");

                        var Total = First + Second + Third;
                        var average = Total / 3;
                        ViewBag.CAverage = ((TFirst + TSecond + TThird) / 3).ToString("0");

                        if (average >= 80)
                        {
                            ViewBag.OverallGrade = "A";
                            ViewBag.OverallRating = "EXCELLENT";
                            ViewBag.Statues = "Promoted";

                        }
                        else if (average >= 70)
                        {
                            ViewBag.OverallGrade = "B";
                            ViewBag.OverallRating = "VERY GOOD";
                            ViewBag.Statues = "Promoted";
                        }
                        else if (average >= 60)
                        {
                            ViewBag.OverallGrade = "C";
                            ViewBag.OverallRating = "GOOD";
                            ViewBag.Statues = "Promoted";
                        }
                        else if (average >= 50)
                        {
                            ViewBag.OverallGrade = "D";
                            ViewBag.OverallRating = "CREDIT";
                            ViewBag.Statues = "Promoted on Trial";
                        }
                        else if (average >= 40)
                        {
                            ViewBag.OverAllGrades = "E";
                            ViewBag.OverallRating = "PASS";
                            ViewBag.Statues = "Promoted on Trial";
                        }
                        else if (average < 40)
                        {
                            ViewBag.OverAllGrades = "F";
                            ViewBag.OverallRating = "FAIL";
                            ViewBag.Statues = "Repeat";
                        }
                    }
                    //return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == session.AcademicYear && s.Term == term.TermName));
                    return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == session.AcademicYear && s.Term == term.TermName));
                }
                else
                {
                    ViewBag.NoDetails = "   No result Found, Please enter Apropriate Search";
                    return View(db.scores.ToList().Where(s => s.StudentNumber == "" && s.AcademicYear == "" && s.Term == ""));
                }
            }
            return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == session.AcademicYear && s.Term == term.TermName));

        }


        //Get index for deleting records
        [Authorize(Roles = "Admin")]
        public ActionResult IndexDelete(string sortOrder, string StudtNumFilter, string StudtNum, string AccademicYearFilter, string AccademicYear, string TermFilter, string Terms, int? page)
        {

            //getting dropdown
            ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");

            if (StudtNum != null && AccademicYear != null && Terms != null)
            {
                //getting Term 
                int termID = Convert.ToInt32(Terms);
                Term term = db.terms.Find(termID);

                //getting Session
                int sessionID = Convert.ToInt32(AccademicYear);
                Session session = db.Sessions.Find(sessionID);

                //getting Student
                Student studt = db.Students.FirstOrDefault(s => s.StudentNumber == StudtNum.Trim());
                if (studt != null)
                {
                    ViewBag.Detail = "This Records is for: " + studt.Surname + " " + studt.OtherName;
                    return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == session.AcademicYear && s.Term == term.TermName && s.Class == studt.Class));
                }
                else
                {
                    ViewBag.NoDetail = "No Student with this Admission in the school, Please ensure the Admission Number is correct";
                    return View(db.scores.ToList().Take(0));
                }

            }

            return View(db.scores.ToList().Take(0));
        }



        //IndexFormMaster for all Score
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult IndexFormMaster(string sortOrder, string StudtNumFilter, string StudtNum, string AccademicYearFilter, string AccademicYear, string TermFilter, string Terms, int? page)
        {

            //CHECKING Logo
            SchoolLogo logo = db.SchoolLogoes.FirstOrDefault();
            if (logo != null)
            {
                ViewBag.Logo = logo.logo;
            }

            //getting Student
            Student studt = db.Students.FirstOrDefault(s => s.StudentNumber == StudtNum.Trim());


            //getting current academic year from global settings 
            GlobalSettings academicyear = db.GlobalSettings.FirstOrDefault(e => e.Name == "Session");
            ViewBag.AccademicYear = academicyear.Value;

            GlobalSettings AccTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "Term");
            ViewBag.Terms = AccTerm.Value;


            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (StudtNum != null && AccademicYear != null && Terms != null)
            {
                page = 1;
            }

            else
            {
                StudtNum = StudtNumFilter;
                AccademicYear = AccademicYearFilter;
                Terms = TermFilter;
            }

            ViewBag.StudtNumFilter = StudtNum;
            ViewBag.AccademicYearFilter = AccademicYear;
            ViewBag.TermFilter = Terms;


            var student = from r in db.scores
                          select r;
            if (!String.IsNullOrEmpty(StudtNum))
            {
                student = student.Where(r => r.StudentNumber.Contains(StudtNum.Trim())
                                       && r.AcademicYear.Contains(AccademicYear)
                                        && r.Term.Contains(Terms));
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


            //getting form master and ensuring that the student is in the form master class
            Teachers user = db.Teachers.FirstOrDefault(t => t.UserName == User.Identity.Name);

            //getting Class Master Class
            ClassList Fclass = db.ClassLists.FirstOrDefault(t => t.FormMaster == user.FullName);
            if (StudtNum == null && AccademicYear == null && Terms == null)
            {
                ViewBag.AcademicYear = "";
                ViewBag.Term = "";
                ViewBag.AdmissionNo = "";
                ViewBag.Surname = "";
                ViewBag.OtherName = "";
                ViewBag.Class = "";
                ViewBag.ObtainableScore = "";
                ViewBag.OverAllGrades = "";
                ViewBag.OverallRating = "";
                //ViewBag.Passport = 0x00;
                ViewBag.EndTerm = "";
                ViewBag.NextTerm = "";
                ViewBag.FessOwed = "";
                ViewBag.FessPaid = "";
                ViewBag.Tclass = "";


                //ViewBag.NoDetails = "No Details Found";
                return View();
                //return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == session.AcademicYear && s.Term == term.TermName));

            }

            else if (StudtNum != null && AccademicYear != null && Terms != null)
            {

                //findind search from Database
                score scoresearch = db.scores.FirstOrDefault(s => s.AcademicYear == academicyear.Value && s.StudentNumber == StudtNum.Trim() && s.Term == Terms);
                if (scoresearch != null && Fclass.ClassName == studt.Class)

                {
                    //Geting View Bag Value to display on the IndexAll
                    score studnt = db.scores.FirstOrDefault(u => u.StudentNumber == StudtNum.Trim());
                    ViewBag.AdmissionNo = studt.StudentNumber;
                    ViewBag.Surname = studt.Surname;
                    ViewBag.OtherName = studt.OtherName;
                    ViewBag.Class = studt.Class;

                    //Accademic Year
                    ViewBag.AcademicYear = academicyear.Value; ;

                    //Term
                    ViewBag.Term = Terms;

                    ViewBag.Category = studt.Category;

                    //getting Ending Term from GlobalSettings
                    GlobalSettings EndTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "EndTerm");
                    ViewBag.EndTerm = EndTerm.Value;

                    //getting Next term from GlobalSettings
                    GlobalSettings NextTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "NextTerm");
                    ViewBag.NextTerm = NextTerm.Value;

                    //getting student passport
                    ViewBag.Passport = studt.Passport;

                    //getting Total number in Class
                    var count = db.Students.Count(s => s.Class == studt.Class && s.Transistion == "No");
                    ViewBag.Tclass = count;

                    //getting  Fee Owed
                    //var c = from p in db.payments
                    //        where p.StudentNum == StudtNum
                    //        select p;
                    //if (c != null)
                    //{
                    //    ViewBag.FeesOwed = c.Sum(t => t.Balance);
                    //}
                    //else
                    //{
                    //    ViewBag.FeesOwed = null;
                    //}

                    ViewBag.FeesOwed = "";

                    //getting Fee Paid
                    Payment payment = db.payments.FirstOrDefault(p => p.StudentNum == StudtNum.Trim() && p.Session == academicyear.Value && p.Term == Terms);

                    //if (payment != null)
                    //{
                    //    ViewBag.FeesPaid = payment.Amount;
                    //}
                    //else
                    //{
                    //    ViewBag.FeesPaid = null;
                    //}
                    ViewBag.FeesPaid = "";

                    if (studt.Category == "Primary" || studt.Category == "Senior" || studt.Category == "Junior")
                    {
                        //getting total Cognitive, Affective, Psychomoto, Exam, Scores 
                        ViewBag.TotalCognitive = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == academicyear.Value && p.Term == Terms).Sum(d => d.FirstCA).ToString("0");
                        ViewBag.TotalAffective = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == academicyear.Value && p.Term == Terms).Sum(d => d.SecondCA).ToString("0");
                        ViewBag.TotalPsychomoto = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == academicyear.Value && p.Term == Terms).Sum(d => d.Psychomoto).ToString("0");
                        ViewBag.TotalExam = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == academicyear.Value && p.Term == Terms).Sum(d => d.Exam);
                        ViewBag.TotalScore = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == academicyear.Value && p.Term == Terms).Sum(d => d.Total).ToString("0");
                    }
                    else if (studt.Category == "Nursery" || studt.Category == "Pre-Nursery")
                    {
                        ViewBag.Tobtainable = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == academicyear.Value && p.Term == Terms).Sum(d => d.ObtainableScore).ToString("0");
                        ViewBag.Tobtained = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == academicyear.Value && p.Term == Terms).Sum(d => d.Exam).ToString("0");
                    }



                    //getting Overall Score for 
                    var x = from d in db.scores
                            where d.StudentNumber == StudtNum && d.AcademicYear == academicyear.Value && d.Term == Terms
                            select d;

                    decimal OverallScores = x.Sum(y => y.Total);
                    var count2 = x.Count();
                    decimal Gradess = OverallScores / count2;

                    //Getting Obtainable Scores
                    ViewBag.ObtainableScore = count2 * 100;

                    if (Gradess >= 80)
                    {
                        ViewBag.OverAllGrades = "A";
                        ViewBag.OverallRating = "EXCELLENT";
                    }
                    else if (Gradess >= 70)
                    {
                        ViewBag.OverAllGrades = "B";
                        ViewBag.OverallRating = "GOOD";
                    }
                    else if (Gradess >= 60)
                    {
                        ViewBag.OverAllGrades = "C";
                        ViewBag.OverallRating = "CREDIT";
                    }
                    else if (Gradess >= 50)
                    {
                        ViewBag.OverAllGrades = "D";
                        ViewBag.OverallRating = "PASS";
                    }
                    else if (Gradess >= 40)
                    {
                        ViewBag.OverAllGrades = "E";
                        ViewBag.OverallRating = "PASS";
                    }
                    else if (Gradess < 40)
                    {
                        ViewBag.OverAllGrades = "F";
                        ViewBag.OverallRating = "FAIL";
                    }


                    //if (term.TermName == "THIRD")
                    //{

                    //    //counting total numbers of subject offerred Per-Term
                    //    var count1st = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "FIRST" && t.AcademicYear == session.AcademicYear).Count();
                    //    var count2nd = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "SECOND" && t.AcademicYear == session.AcademicYear).Count();
                    //    var count3rd = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "THIRD" && t.AcademicYear == session.AcademicYear).Count();

                    //    //calculating cumulative scores for third term
                    //    var TFirst = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "FIRST" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);
                    //    var TSecond = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "SECOND" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);
                    //    var TThird = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "THIRD" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);

                    //    //calculating overall score for each Term
                    //    var First = TFirst / count1st;
                    //    var Second = TSecond / count2nd;
                    //    var Third = TThird / count3rd;

                    //    //total score for first, Second, Third Term of a Session
                    //    ViewBag.FirstTerm = TFirst.ToString("0");
                    //    ViewBag.SecondTerm = TSecond.ToString("0");
                    //    ViewBag.ThirdTerm = TThird.ToString("0");

                    //    var Total = First + Second + Third;
                    //    var average = Total / 3;
                    //    ViewBag.CAverage = ((TFirst + TSecond + TThird) / 3).ToString("0");

                    //    if (average >= 80)
                    //    {
                    //        ViewBag.OverallGrade = "A";
                    //        ViewBag.OverallRating = "EXCELLENT";
                    //        ViewBag.Statues = "Promoted";

                    //    }
                    //    else if (average >= 70)
                    //    {
                    //        ViewBag.OverallGrade = "B";
                    //        ViewBag.OverallRating = "VERY GOOD";
                    //        ViewBag.Statues = "Promoted";
                    //    }
                    //    else if (average >= 60)
                    //    {
                    //        ViewBag.OverallGrade = "C";
                    //        ViewBag.OverallRating = "GOOD";
                    //        ViewBag.Statues = "Promoted";
                    //    }
                    //    else if (average >= 50)
                    //    {
                    //        ViewBag.OverallGrade = "D";
                    //        ViewBag.OverallRating = "CREDIT";
                    //        ViewBag.Statues = "Promoted on Trial";
                    //    }
                    //    else if (average >= 40)
                    //    {
                    //        ViewBag.OverAllGrades = "E";
                    //        ViewBag.OverallRating = "PASS";
                    //        ViewBag.Statues = "Promoted on Trial";
                    //    }
                    //    else if (average < 40)
                    //    {
                    //        ViewBag.OverAllGrades = "F";
                    //        ViewBag.OverallRating = "FAIL";
                    //        ViewBag.Statues = "Repeat";
                    //    }
                    //}

                }
                else
                {
                    ViewBag.NoDetails = "Please you can not print this result, as the student may not be in your Class or No Score has been entered for this student";
                    return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == "" && s.AcademicYear == "" && s.Term == ""));
                }
                return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == AccademicYear && s.Term == Terms));
            }
            return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == AccademicYear && s.Term == Terms));
        }

        //IndexFormMaster for all Score
        [Authorize(Roles = "HOD")]
        public ActionResult IndexHod(string sortOrder, string StudtNumFilter, string StudtNum, string AccademicYearFilter, string AccademicYear, string TermFilter, string Terms, int? page, string Message)
        {

            //CHECKING Logo
            SchoolLogo logo = db.SchoolLogoes.FirstOrDefault();
            if (logo != null)
            {
                ViewBag.Logo = logo.logo;
            }
            //getting Student
            Student studt = db.Students.FirstOrDefault(s => s.StudentNumber == StudtNum.Trim());

            //getting dropdown
            ViewBag.AccademicYear = new SelectList(db.Sessions, "AcademicYear", "AcademicYear");
            ViewBag.Terms = new SelectList(db.terms, "TermName", "TermName");

            ViewBag.Message = Message;


            //getting form master and ensuring that the student is in the form master class
            Teachers user = db.Teachers.FirstOrDefault(t => t.UserName == User.Identity.Name);

            //getting Class Master Class
            HOD Fclass = db.Hod.FirstOrDefault(t => t.StaffNumber == user.StaffNumber);


            if (StudtNum == null && AccademicYear == null && Terms == null)
            {
                ViewBag.AcademicYear = "";
                ViewBag.Term = "";
                ViewBag.AdmissionNo = "";
                ViewBag.Surname = "";
                ViewBag.OtherName = "";
                ViewBag.Class = "";
                ViewBag.ObtainableScore = "";
                ViewBag.OverAllGrades = "";
                ViewBag.OverallRating = "";
                //ViewBag.Passport = 0x00;
                ViewBag.EndTerm = "";
                ViewBag.NextTerm = "";
                ViewBag.FessOwed = "";
                ViewBag.FessPaid = "";
                ViewBag.Tclass = "";

                return View(db.scores.ToList().Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == AccademicYear && s.Term == Terms));

            }

            else if (StudtNum != null && AccademicYear != null && Terms != null)
            {

                TempData["Term"] = Terms;
                TempData["Session"] = AccademicYear;
                TempData["No"] = StudtNum;
                ViewBag.AccademicYear = new SelectList(db.Sessions.OrderByDescending(t => t.AcademicYear == AccademicYear), "AcademicYear", "AcademicYear");
                ViewBag.Terms = new SelectList(db.terms.OrderByDescending(t => t.TermName == Terms), "TermName", "TermName");
                //finding search from Database
                score scoresearch = db.scores.FirstOrDefault(s => s.AcademicYear == AccademicYear && s.StudentNumber == StudtNum.Trim() && s.Term == Terms);
                if (scoresearch != null /*&& Fclass.Category == studt.Session*/)
                {
                    //Geting View Bag Value to display on the IndexAll
                    score studnt = db.scores.FirstOrDefault(u => u.StudentNumber == StudtNum.Trim());
                    ViewBag.AdmissionNo = studt.StudentNumber;
                    ViewBag.Surname = studt.Surname;
                    ViewBag.OtherName = studt.OtherName;
                    ViewBag.Class = studt.Class;

                    //Accademic Year
                    ViewBag.AcademicYear = AccademicYear;

                    //Term
                    ViewBag.Term = Terms;
                    ViewBag.Verify = scoresearch.HODVerify;

                    ViewBag.Category = studt.Category;

                    //getting Ending Term from GlobalSettings
                    GlobalSettings EndTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "EndTerm");
                    ViewBag.EndTerm = EndTerm.Value;

                    //getting Next term from GlobalSettings
                    GlobalSettings NextTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "NextTerm");
                    ViewBag.NextTerm = NextTerm.Value;

                    //getting student passport
                    ViewBag.Passport = studt.Passport;

                    //getting Total number in Class
                    var count = db.Students.Count(s => s.Class == studt.Class && s.Transistion == "No");
                    ViewBag.Tclass = count;

                    //getting  Fee Owed
                    //var c = from p in db.payments
                    //        where p.StudentNum == StudtNum
                    //        select p;
                    //if (c != null)
                    //{
                    //    ViewBag.FeesOwed = c.Sum(t => t.Balance);
                    //}
                    //else
                    //{
                    //    ViewBag.FeesOwed = null;
                    //}

                    ViewBag.FeesOwed = "";

                    //getting Fee Paid
                    Payment payment = db.payments.FirstOrDefault(p => p.StudentNum == StudtNum.Trim() && p.Session == AccademicYear && p.Term == Terms);

                    //if (payment != null)
                    //{
                    //    ViewBag.FeesPaid = payment.Amount;
                    //}
                    //else
                    //{
                    //    ViewBag.FeesPaid = null;
                    //}
                    ViewBag.FeesPaid = "";

                    if (studt.Category == "Primary" || studt.Category == "Senior" || studt.Category == "Junior")
                    {
                        //getting total Cognitive, Affective, Psychomoto, Exam, Scores 
                        ViewBag.TotalCognitive = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.FirstCA).ToString("0");
                        ViewBag.TotalAffective = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.SecondCA).ToString("0");
                        ViewBag.TotalPsychomoto = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.Psychomoto).ToString("0");
                        ViewBag.TotalExam = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.Exam);
                        ViewBag.TotalScore = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.Total).ToString("0");
                    }
                    else if (studt.Category == "Nursery" || studt.Category == "Pre-Nursery")
                    {
                        ViewBag.Tobtainable = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.ObtainableScore).ToString("0");
                        ViewBag.Tobtained = db.scores.Where(p => p.StudentNumber == StudtNum.Trim() && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.Exam).ToString("0");
                    }



                    //getting Overall Score for Student
                    score studnt1 = db.scores.FirstOrDefault(u => u.StudentNumber == StudtNum.Trim());

                    var x = from d in db.scores
                            where d.StudentNumber == studt.StudentNumber && d.AcademicYear == AccademicYear && d.Term == Terms
                            select d;

                    decimal OverallScores = x.Sum(y => y.Total);
                    var count2 = x.Count();
                    decimal Gradess = OverallScores / count2;

                    //Getting Obtainable Scores
                    ViewBag.ObtainableScore = count2 * 100;

                    if (Gradess >= 80)
                    {
                        ViewBag.OverAllGrades = "A";
                        ViewBag.OverallRating = "EXCELLENT";
                    }
                    else if (Gradess >= 70)
                    {
                        ViewBag.OverAllGrades = "B";
                        ViewBag.OverallRating = "GOOD";
                    }
                    else if (Gradess >= 60)
                    {
                        ViewBag.OverAllGrades = "C";
                        ViewBag.OverallRating = "CREDIT";
                    }
                    else if (Gradess >= 50)
                    {
                        ViewBag.OverAllGrades = "D";
                        ViewBag.OverallRating = "PASS";
                    }
                    else if (Gradess >= 40)
                    {
                        ViewBag.OverAllGrades = "E";
                        ViewBag.OverallRating = "PASS";
                    }
                    else if (Gradess < 40)
                    {
                        ViewBag.OverAllGrades = "F";
                        ViewBag.OverallRating = "FAIL";
                    }


                    //if (term.TermName == "THIRD")
                    //{

                    //    //counting total numbers of subject offerred Per-Term
                    //    var count1st = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "FIRST" && t.AcademicYear == session.AcademicYear).Count();
                    //    var count2nd = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "SECOND" && t.AcademicYear == session.AcademicYear).Count();
                    //    var count3rd = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "THIRD" && t.AcademicYear == session.AcademicYear).Count();

                    //    //calculating cumulative scores for third term
                    //    var TFirst = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "FIRST" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);
                    //    var TSecond = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "SECOND" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);
                    //    var TThird = db.scores.Where(t => t.StudentNumber == StudtNum.Trim() && t.Term == "THIRD" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);

                    //    //calculating overall score for each Term
                    //    var First = TFirst / count1st;
                    //    var Second = TSecond / count2nd;
                    //    var Third = TThird / count3rd;

                    //    //total score for first, Second, Third Term of a Session
                    //    ViewBag.FirstTerm = TFirst.ToString("0");
                    //    ViewBag.SecondTerm = TSecond.ToString("0");
                    //    ViewBag.ThirdTerm = TThird.ToString("0");

                    //    var Total = First + Second + Third;
                    //    var average = Total / 3;
                    //    ViewBag.CAverage = ((TFirst + TSecond + TThird) / 3).ToString("0");

                    //    if (average >= 80)
                    //    {
                    //        ViewBag.OverallGrade = "A";
                    //        ViewBag.OverallRating = "EXCELLENT";
                    //        ViewBag.Statues = "Promoted";

                    //    }
                    //    else if (average >= 70)
                    //    {
                    //        ViewBag.OverallGrade = "B";
                    //        ViewBag.OverallRating = "VERY GOOD";
                    //        ViewBag.Statues = "Promoted";
                    //    }
                    //    else if (average >= 60)
                    //    {
                    //        ViewBag.OverallGrade = "C";
                    //        ViewBag.OverallRating = "GOOD";
                    //        ViewBag.Statues = "Promoted";
                    //    }
                    //    else if (average >= 50)
                    //    {
                    //        ViewBag.OverallGrade = "D";
                    //        ViewBag.OverallRating = "CREDIT";
                    //        ViewBag.Statues = "Promoted on Trial";
                    //    }
                    //    else if (average >= 40)
                    //    {
                    //        ViewBag.OverAllGrades = "E";
                    //        ViewBag.OverallRating = "PASS";
                    //        ViewBag.Statues = "Promoted on Trial";
                    //    }
                    //    else if (average < 40)
                    //    {
                    //        ViewBag.OverAllGrades = "F";
                    //        ViewBag.OverallRating = "FAIL";
                    //        ViewBag.Statues = "Repeat";
                    //    }
                    //}

                    return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == AccademicYear && s.Term == Terms));

                }
                else
                {
                    ViewBag.NoDetails = "Please you can not Verify this result, the student may not be in your Session or No Score has been entered for this student";
                    return View(db.scores.ToList().Where(s => s.StudentNumber == "" && s.AcademicYear == "" && s.Term == ""));
                }

            }
            return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == AccademicYear && s.Term == Terms));
        }


        //Index Parent to check thier Children Result
        public ActionResult IndexParent(string sortOrder, string StudtNumFilter, string StudtNum, string AccademicYearFilter, string AccademicYear, string TermFilter, string Terms, int? page, string phone)
        {
            phone = User.Identity.Name;

            //CHECKING Logo
            SchoolLogo logo = db.SchoolLogoes.FirstOrDefault();
            if (logo != null)
            {
                ViewBag.Logo = logo.logo;
            }

            //getting the list of Student Number that contain the phone numbers entered
            ViewBag.AdNum = new SelectList(db.Students.Where(f => f.PhoneNumber == phone.Trim()), "StudentNumber", "StudentNumber");

            //getting current academic year from global settings 
            GlobalSettings academicyear = db.GlobalSettings.FirstOrDefault(e => e.Name == "Session");
            ViewBag.AccademicYear = academicyear.Value;

            //ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.Terms = new SelectList(db.terms, "TermName", "TermName");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


            if (StudtNum != null && AccademicYear != null && Terms != null)
            {
                page = 1;
                ViewBag.Phone = phone;
            }

            else
            {
                StudtNum = StudtNumFilter;
                AccademicYear = AccademicYearFilter;
                Terms = TermFilter;
            }

            ViewBag.StudtNumFilter = StudtNum;
            ViewBag.AccademicYearFilter = AccademicYear;
            ViewBag.TermFilter = Terms;

            var student = from r in db.scores
                          select r;
            if (!String.IsNullOrEmpty(StudtNum))
            {
                student = student.Where(r => r.StudentNumber.Contains(StudtNum.Trim())
                                       && r.AcademicYear.Contains(AccademicYear)
                                        && r.Term.Contains(Terms));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    student = student.OrderByDescending(r => r.StudentNumber);
                    break;
                default:  // Name ascending 
                    student = student.OrderBy(r => r.StudentNumber);
                    break;
            }


            if (StudtNum == null)
            {
                return View(db.scores.ToList().Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == "" && s.Term == "").Take(0));
            }


            //searching student with phone number
            Student getstudent = db.Students.FirstOrDefault(s => s.PhoneNumber == phone && s.StudentNumber == StudtNum);
            if (getstudent != null)
            {
                if (getstudent.CheckResult == "Allow")
                {

                    //findind search from Database
                    score scoresearch = db.scores.FirstOrDefault(s => s.AcademicYear == AccademicYear && s.StudentNumber == getstudent.StudentNumber && s.Term == Terms);
                    if (scoresearch != null && scoresearch.HODVerify == "Verified")
                    {
                        if (StudtNum == null && AccademicYear == null && Terms == null)
                        {
                            ViewBag.AcademicYear = "";
                            ViewBag.term = "";
                            ViewBag.AdmissionNo = "";
                            ViewBag.Surname = "";
                            ViewBag.OtherName = "";
                            ViewBag.Class = "";
                            ViewBag.ObtainableScore = "";
                            ViewBag.OverAllGrades = "";
                            ViewBag.OverallRating = "";
                            //ViewBag.Passport = 0x00;
                            ViewBag.EndTerm = "";
                            ViewBag.NextTerm = "";
                            ViewBag.FessOwed = "";
                            ViewBag.FessPaid = "";
                            ViewBag.Tclass = "";


                            ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AccademicYear");
                            ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");

                            ViewBag.NoDetails = "No Details Found";
                            return View(db.scores.ToList().Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == "" && s.Term == ""));

                        }

                        else if (StudtNum != null && AccademicYear != null && Terms != null)
                        {
                            if (scoresearch != null)
                            {

                                //Geting View Bag Value to display on the IndexAll
                                score studnt = db.scores.FirstOrDefault(u => u.StudentNumber == getstudent.StudentNumber);
                                ViewBag.AdmissionNo = getstudent.StudentNumber;
                                ViewBag.Surname = getstudent.Surname;
                                ViewBag.OtherName = getstudent.OtherName;
                                ViewBag.Class = getstudent.Class;

                                //Accademic Year
                                ViewBag.AcademicYear = AccademicYear;

                                //Term
                                ViewBag.Term = Terms;

                                ViewBag.Category = getstudent.Category;
                                ViewBag.Passport = getstudent.Passport;

                                //getting Ending Term from GlobalSettings
                                GlobalSettings EndTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "EndTerm");
                                ViewBag.EndTerm = EndTerm.Value;

                                //getting Next term from GlobalSettings
                                GlobalSettings NextTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "NextTerm");
                                ViewBag.NextTerm = NextTerm.Value;

                                //getting student passport
                                ViewBag.Passport = getstudent.Passport;

                                //getting Total number in Class
                                var count = db.Students.Count(s => s.Class == getstudent.Class && s.Transistion == "No");
                                ViewBag.Tclass = count;

                                //getting  Fee Owed
                                //var c = from p in db.payments
                                //        where p.StudentNum == StudtNum
                                //        select p;
                                //if (c != null)
                                //{
                                //    ViewBag.FeesOwed = c.Sum(t => t.Balance);
                                //}
                                //else
                                //{
                                //    ViewBag.FeesOwed = null;
                                //}

                                ViewBag.FeesOwed = "";

                                //getting Fee Paid
                                Payment payment = db.payments.FirstOrDefault(p => p.StudentNum == getstudent.StudentNumber && p.Session == AccademicYear && p.Term == Terms);

                                //if (payment != null)
                                //{
                                //    ViewBag.FeesPaid = payment.Amount;
                                //}
                                //else
                                //{
                                //    ViewBag.FeesPaid = null;
                                //}
                                ViewBag.FeesPaid = "";

                                //DROPDOWNLIST
                                ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
                                ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");



                                if (getstudent.Category == "Primary" || getstudent.Category == "Senior" || getstudent.Category == "Junior")
                                {
                                    //getting total Cognitive, Affective, Psychomoto, Exam, Scores 
                                    ViewBag.TotalCognitive = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.FirstCA).ToString("0");
                                    ViewBag.TotalAffective = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.SecondCA).ToString("0");
                                    ViewBag.TotalPsychomoto = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.Psychomoto).ToString("0");
                                    ViewBag.TotalExam = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.Exam);
                                    ViewBag.TotalScore = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.Total).ToString("0");
                                }
                                else if (getstudent.Category == "Nursery" || getstudent.Category == "Pre-Nursery")
                                {
                                    ViewBag.Tobtainable = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.ObtainableScore).ToString("0");
                                    ViewBag.Tobtained = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == AccademicYear && p.Term == Terms).Sum(d => d.Exam).ToString("0");
                                }



                                //getting Overall Score for Student
                                score studnt1 = db.scores.FirstOrDefault(u => u.StudentNumber == getstudent.StudentNumber);

                                var x = from d in db.scores
                                        where d.StudentNumber == getstudent.StudentNumber && d.AcademicYear == AccademicYear && d.Term == Terms
                                        select d;

                                decimal OverallScores = x.Sum(y => y.Total);
                                var count2 = x.Count();
                                decimal Gradess = OverallScores / count2;

                                //Getting Obtainable Scores
                                ViewBag.ObtainableScore = count2 * 100;

                                if (Gradess >= 80)
                                {
                                    ViewBag.OverAllGrades = "A";
                                    ViewBag.OverallRating = "EXCELLENT";
                                }
                                else if (Gradess >= 70)
                                {
                                    ViewBag.OverAllGrades = "B";
                                    ViewBag.OverallRating = "GOOD";
                                }
                                else if (Gradess >= 60)
                                {
                                    ViewBag.OverAllGrades = "C";
                                    ViewBag.OverallRating = "CREDIT";
                                }
                                else if (Gradess >= 50)
                                {
                                    ViewBag.OverAllGrades = "D";
                                    ViewBag.OverallRating = "PASS";
                                }
                                else if (Gradess >= 40)
                                {
                                    ViewBag.OverAllGrades = "E";
                                    ViewBag.OverallRating = "PASS";
                                }
                                else if (Gradess < 40)
                                {
                                    ViewBag.OverAllGrades = "F";
                                    ViewBag.OverallRating = "FAIL";
                                }


                                if (Terms == "THIRD")
                                {

                                    //counting total numbers of subject offerred Per-Term
                                    var count1st = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "FIRST" && t.AcademicYear == AccademicYear).Count();
                                    var count2nd = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "SECOND" && t.AcademicYear == AccademicYear).Count();
                                    var count3rd = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "THIRD" && t.AcademicYear == AccademicYear).Count();

                                    //calculating cumulative scores for third term
                                    var TFirst = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "FIRST" && t.AcademicYear == AccademicYear).Sum(r => r.Total);
                                    var TSecond = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "SECOND" && t.AcademicYear == AccademicYear).Sum(r => r.Total);
                                    var TThird = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "THIRD" && t.AcademicYear == AccademicYear).Sum(r => r.Total);

                                    //calculating overall score for each Term
                                    var First = TFirst / count1st;
                                    var Second = TSecond / count2nd;
                                    var Third = TThird / count3rd;

                                    //total score for first, Second, Third Term of a Session
                                    ViewBag.FirstTerm = TFirst.ToString("0");
                                    ViewBag.SecondTerm = TSecond.ToString("0");
                                    ViewBag.ThirdTerm = TThird.ToString("0");

                                    var Total = First + Second + Third;
                                    var average = Total / 3;
                                    ViewBag.CAverage = ((TFirst + TSecond + TThird) / 3).ToString("0");

                                    if (average >= 80)
                                    {
                                        ViewBag.OverallGrade = "A";
                                        ViewBag.OverallRating = "EXCELLENT";
                                        ViewBag.Statues = "Promoted";

                                    }
                                    else if (average >= 70)
                                    {
                                        ViewBag.OverallGrade = "B";
                                        ViewBag.OverallRating = "VERY GOOD";
                                        ViewBag.Statues = "Promoted";
                                    }
                                    else if (average >= 60)
                                    {
                                        ViewBag.OverallGrade = "C";
                                        ViewBag.OverallRating = "GOOD";
                                        ViewBag.Statues = "Promoted";
                                    }
                                    else if (average >= 50)
                                    {
                                        ViewBag.OverallGrade = "D";
                                        ViewBag.OverallRating = "CREDIT";
                                        ViewBag.Statues = "Promoted on Trial";
                                    }
                                    else if (average >= 40)
                                    {
                                        ViewBag.OverAllGrades = "E";
                                        ViewBag.OverallRating = "PASS";
                                        ViewBag.Statues = "Promoted on Trial";
                                    }
                                    else if (average < 40)
                                    {
                                        ViewBag.OverAllGrades = "F";
                                        ViewBag.OverallRating = "FAIL";
                                        ViewBag.Statues = "Repeat";
                                    }
                                }

                                var scores = db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == getstudent.StudentNumber && s.AcademicYear == AccademicYear && s.Term == Terms);
                                //return new ViewAsPdf("IndexParent", scores);
                                return View(scores);
                            }

                        }

                    }
                    else
                    {
                        ViewBag.NoDetails = "Please you can not check result now, Scores may not have been entered yet or Result as not yet been verified. Please contact the school Administrator on  +234 ( 0)704 0079 595";
                        var Scoress = db.scores.ToList().Where(s => s.StudentNumber == " " && s.AcademicYear == " " && s.Term == " ");
                        return View(Scoress);
                    }
                }
                else
                {

                    ViewBag.Phone = phone;
                    ViewBag.NoDetails = "Please you do not have access to view this result as your child school fees may not have been fully paid. Please contact the school Administrator on  +234 ( 0)704 0079 595";
                }
            }
            else
            {
                ViewBag.Phone = phone;
                ViewBag.NoDetails = "To check Your Child Result, Select Child Admission Number (e.g LWA 89/15) and Term (e.g FIRST)";
                var Score = db.scores.ToList().Where(s => s.StudentNumber == " " && s.AcademicYear == " " && s.Term == " ");
                return View(Score);
            }
            ViewBag.Phone = phone;
            var Scores = db.scores.ToList().Where(s => s.StudentNumber == " " && s.AcademicYear == " " && s.Term == " ");
            return View(Scores);
        }


        // index check result for parent
        //public ActionResult CheckResult(string Code)
        //{
        //    ParentCode checkcode = db.ParentCodes.FirstOrDefault(t => t.codes == Code);
        //    if (checkcode != null)
        //    {
        //        //getting student informations
        //        Student getstudent = db.Students.FirstOrDefault(r => r.StudentNumber == checkcode.StudentNumber);

        //        //Accademic Year
        //        GlobalSettings getses = db.GlobalSettings.FirstOrDefault(t => t.Name == "Session");

        //        //Term
        //        GlobalSettings gettermd = db.GlobalSettings.FirstOrDefault(t => t.Name == "Term");

        //        //checking if scores exist for the present term
        //        score studntScores = db.scores.FirstOrDefault(s => s.StudentNumber == getstudent.StudentNumber && s.AcademicYear == getses.Value && s.Term == gettermd.Value);

        //        if (studntScores != null && studntScores.HODVerify == "Verified")
        //        {
        //            if (getstudent.CheckResult == "Allow")
        //            {

        //                //Geting View Bag Value to display on the IndexAll
        //                ViewBag.AdmissionNo = getstudent.StudentNumber;
        //                ViewBag.Surname = getstudent.Surname;
        //                ViewBag.OtherName = getstudent.OtherName;
        //                ViewBag.Class = getstudent.Class;

        //                //Accademic Year
        //                GlobalSettings getsession = db.GlobalSettings.FirstOrDefault(t => t.Name == "Session");
        //                ViewBag.AcademicYear = getsession.Value;

        //                //Term
        //                GlobalSettings getterm = db.GlobalSettings.FirstOrDefault(t => t.Name == "Term");
        //                ViewBag.Term = getterm.Value;

        //                ViewBag.Category = getstudent.Category;
        //                ViewBag.Passport = getstudent.Passport;

        //                //getting Ending Term from GlobalSettings
        //                GlobalSettings EndTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "EndTerm");
        //                ViewBag.EndTerm = EndTerm.Value;

        //                //getting Next term from GlobalSettings
        //                GlobalSettings NextTerm = db.GlobalSettings.FirstOrDefault(e => e.Name == "NextTerm");
        //                ViewBag.NextTerm = NextTerm.Value;

        //                //getting student passport
        //                ViewBag.Passport = getstudent.Passport;


        //                //getting Total number in Class
        //                var count = db.Students.Count(s => s.Class == getstudent.Class);
        //                ViewBag.Tclass = count;

        //                //getting  Fee Owed
        //                //var c = from p in db.payments
        //                //        where p.StudentNum == StudtNum
        //                //        select p;
        //                //if (c != null)
        //                //{
        //                //    ViewBag.FeesOwed = c.Sum(t => t.Balance);
        //                //}
        //                //else
        //                //{
        //                //    ViewBag.FeesOwed = null;
        //                //}

        //                ViewBag.FeesOwed = "";

        //                //getting Fee Paid
        //                Payment payment = db.payments.FirstOrDefault(p => p.StudentNum == getstudent.StudentNumber && p.Session == getsession.Value && p.Term == getterm.Value);

        //                //if (payment != null)
        //                //{
        //                //    ViewBag.FeesPaid = payment.Amount;
        //                //}
        //                //else
        //                //{
        //                //    ViewBag.FeesPaid = null;
        //                //}
        //                ViewBag.FeesPaid = "";

        //                //DROPDOWNLIST
        //                ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
        //                ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");



        //                if (getstudent.Category == "Primary" || getstudent.Category == "Senior" || getstudent.Category == "Junior")
        //                {
        //                    //getting total Cognitive, Affective, Psychomoto, Exam, Scores 
        //                    ViewBag.TotalCognitive = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == getsession.Value && p.Term == getterm.Value).Sum(d => d.FirstCA).ToString("0");
        //                    ViewBag.TotalAffective = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == getsession.Value && p.Term == getterm.Value).Sum(d => d.SecondCA).ToString("0");
        //                    ViewBag.TotalPsychomoto = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == getsession.Value && p.Term == getterm.Value).Sum(d => d.Psychomoto).ToString("0");
        //                    ViewBag.TotalExam = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == getsession.Value && p.Term == getterm.Value).Sum(d => d.Exam);
        //                    ViewBag.TotalScore = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == getsession.Value && p.Term == getterm.Value).Sum(d => d.Total).ToString("0");
        //                }
        //                else if (getstudent.Category == "Nursery" || getstudent.Category == "Pre-Nursery")
        //                {
        //                    ViewBag.Tobtainable = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == getsession.Value && p.Term == getterm.Value).Sum(d => d.ObtainableScore).ToString("0");
        //                    ViewBag.Tobtained = db.scores.Where(p => p.StudentNumber == getstudent.StudentNumber && p.AcademicYear == getsession.Value && p.Term == getterm.Value).Sum(d => d.Exam).ToString("0");
        //                }



        //                //getting Overall Score for Student
        //                score studnt1 = db.scores.FirstOrDefault(u => u.StudentNumber == getstudent.StudentNumber);

        //                var x = from d in db.scores
        //                        where d.StudentNumber == studnt1.StudentNumber && d.AcademicYear == studnt1.AcademicYear && d.Term == getterm.Value
        //                        select d;

        //                decimal OverallScores = x.Sum(y => y.Total);
        //                var count2 = x.Count();
        //                decimal Gradess = OverallScores / count2;

        //                //Getting Obtainable Scores
        //                ViewBag.ObtainableScore = count2 * 100;

        //                if (Gradess >= 80)
        //                {
        //                    ViewBag.OverAllGrades = "A";
        //                    ViewBag.OverallRating = "EXCELLENT";
        //                }
        //                else if (Gradess >= 70)
        //                {
        //                    ViewBag.OverAllGrades = "B";
        //                    ViewBag.OverallRating = "VERY GOOD";
        //                }
        //                else if (Gradess >= 60)
        //                {
        //                    ViewBag.OverAllGrades = "C";
        //                    ViewBag.OverallRating = "GOOD";
        //                }
        //                else if (Gradess >= 50)
        //                {
        //                    ViewBag.OverAllGrades = "D";
        //                    ViewBag.OverallRating = "CREDIT";
        //                }
        //                else if (Gradess >= 40)
        //                {
        //                    ViewBag.OverAllGrades = "E";
        //                    ViewBag.OverallRating = "PASS";
        //                }
        //                else if (Gradess < 40)
        //                {
        //                    ViewBag.OverAllGrades = "F";
        //                    ViewBag.OverallRating = "FAIL";
        //                }


        //                //if (term.TermName == "THIRD")
        //                //{

        //                //    //counting total numbers of subject offerred Per-Term
        //                //    var count1st = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "FIRST" && t.AcademicYear == getsession.Value).Count();
        //                //    var count2nd = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "SECOND" && t.AcademicYear == getsession.Value).Count();
        //                //    var count3rd = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "THIRD" && t.AcademicYear == getsession.Value).Count();

        //                //    //calculating cumulative scores for third term
        //                //    var TFirst = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "FIRST" && t.AcademicYear ==getsession.Value).Sum(r => r.Total);
        //                //    var TSecond = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "SECOND" && t.AcademicYear == getsession.Value).Sum(r => r.Total);
        //                //    var TThird = db.scores.Where(t => t.StudentNumber == getstudent.StudentNumber && t.Term == "THIRD" && t.AcademicYear == getsession.Value).Sum(r => r.Total);

        //                //    //calculating overall score for each Term
        //                //    var First = TFirst / count1st;
        //                //    var Second = TSecond / count2nd;
        //                //    var Third = TThird / count3rd;

        //                //    //total score for first, Second, Third Term of a Session
        //                //    ViewBag.FirstTerm = TFirst.ToString("0");
        //                //    ViewBag.SecondTerm = TSecond.ToString("0");
        //                //    ViewBag.ThirdTerm = TThird.ToString("0");

        //                //    var Total = First + Second + Third;
        //                //    var average = Total / 3;
        //                //    ViewBag.CAverage = ((TFirst + TSecond + TThird) / 3).ToString("0");

        //                //    if (average >= 80)
        //                //    {
        //                //        ViewBag.OverallGrade = "A";
        //                //        ViewBag.OverallRating = "EXCELLENT";
        //                //        ViewBag.Statues = "Promoted";

        //                //    }
        //                //    else if (average >= 70)
        //                //    {
        //                //        ViewBag.OverallGrade = "B";
        //                //        ViewBag.OverallRating = "VERY GOOD";
        //                //        ViewBag.Statues = "Promoted";
        //                //    }
        //                //    else if (average >= 60)
        //                //    {
        //                //        ViewBag.OverallGrade = "C";
        //                //        ViewBag.OverallRating = "GOOD";
        //                //        ViewBag.Statues = "Promoted";
        //                //    }
        //                //    else if (average >= 50)
        //                //    {
        //                //        ViewBag.OverallGrade = "D";
        //                //        ViewBag.OverallRating = "CREDIT";
        //                //        ViewBag.Statues = "Promoted on Trial";
        //                //    }
        //                //    else if (average >= 40)
        //                //    {
        //                //        ViewBag.OverAllGrades = "E";
        //                //        ViewBag.OverallRating = "PASS";
        //                //        ViewBag.Statues = "Promoted on Trial";
        //                //    }
        //                //    else if (average < 40)
        //                //    {
        //                //        ViewBag.OverAllGrades = "F";
        //                //        ViewBag.OverallRating = "FAIL";
        //                //        ViewBag.Statues = "Repeat";
        //                //    }
        //                //}

        //                return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == getstudent.StudentNumber && s.AcademicYear == getsession.Value && s.Term == getterm.Value));

        //            }
        //            else
        //            {
        //                ViewBag.Message = "Your Child result is not available for view, please contact the school administrator:  +234 ( 0)704 0079 595";
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Scores as not yet been entered, Please check back Later, or result may have not yet been verified by HOD";
        //        }
        //    }
        //    else
        //    {

        //    }
        //    return View();
        //}

        //Index for previous results
        public ActionResult IndexPreviousResult(string sortOrder, string StudtNumFilter, string StudtNum, string AccademicYearFilter, string AccademicYear, string TermFilter, string Terms, int? page)
        {

            //CHECKING Logo
            SchoolLogo logo = db.SchoolLogoes.FirstOrDefault();
            if (logo != null)
            {
                ViewBag.Logo = logo.logo;
            }

            //getting Term 
            int termID = Convert.ToInt32(Terms);
            Term term = db.terms.Find(termID);

            //getting the list of Student Number that contain the phone numbers entered
            if (User.IsInRole("Parent"))
            {
                ViewBag.AdNum = new SelectList(db.Students.Where(f => f.PhoneNumber == User.Identity.Name), "StudentNumber", "StudentNumber");
            }


            //getting Session
            int sessionID = Convert.ToInt32(AccademicYear);
            Session session = db.Sessions.Find(sessionID);

            //getting dropdown
            ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (StudtNum != null && AccademicYear != null && Terms != null)
            {
                page = 1;
            }

            else
            {
                StudtNum = StudtNumFilter;
                AccademicYear = AccademicYearFilter;
                Terms = TermFilter;
            }

            ViewBag.StudtNumFilter = StudtNum;
            ViewBag.AccademicYearFilter = AccademicYear;
            ViewBag.TermFilter = Terms;

            //var ivadostaff = db.IvadoStaff.Include(i => i.Branch).Include(i => i.Position);

            ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");


            var student = from r in db.scores
                          select r;
            if (!String.IsNullOrEmpty(StudtNum))
            {
                student = student.Where(r => r.StudentNumber.Contains(StudtNum)
                                       && r.AcademicYear.Contains(AccademicYear)
                                        && r.Term.Contains(Terms));
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



            if (StudtNum == null && AccademicYear == null && Terms == null)
            {
                ViewBag.AcademicYear = "";
                ViewBag.Term = "";
                ViewBag.AdmissionNo = "";
                ViewBag.Surname = "";
                ViewBag.OtherName = "";
                ViewBag.Class = "";
                ViewBag.ObtainableScore = "";
                ViewBag.OverAllGrades = "";
                ViewBag.OverallRating = "";

                ViewBag.NoDetails = "No Details Found";
                return View(db.scores.ToList().Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == session.AcademicYear && s.Term == term.TermName));

            }

            else if (StudtNum != null && AccademicYear != null && Terms != null)
            {

                //findind search from Database
                score scoresearch = db.scores.FirstOrDefault(s => s.AcademicYear == session.AcademicYear && s.StudentNumber == StudtNum.Trim() && s.Term == term.TermName);
                if (scoresearch != null)
                {
                    //Geting View Bag Value to display on the IndexAll
                    score studnt = db.scores.FirstOrDefault(u => u.StudentNumber == StudtNum.Trim());
                    ViewBag.AdmissionNo = studnt.StudentNumber;
                    ViewBag.Surname = studnt.Surname;
                    ViewBag.OtherName = studnt.OtherName;
                    ViewBag.Class = studnt.Class;

                    //Accademic Year
                    ViewBag.AcademicYear = session.AcademicYear;

                    //Term
                    ViewBag.Term = term.TermName;


                    //getting  Fee Owed
                    //var c = from p in db.payments
                    //        where p.StudentNum == StudtNum
                    //        select p;
                    //if (c != null)
                    //{
                    //    ViewBag.FeesOwed = c.Sum(t => t.Balance);
                    //}
                    //else
                    //{
                    //    ViewBag.FeesOwed = null;
                    //}

                    ViewBag.FeesOwed = "";

                    //getting Fee Paid
                    Payment payment = db.payments.FirstOrDefault(p => p.StudentNum == StudtNum && p.Session == session.AcademicYear && p.Term == term.TermName);

                    //if (payment != null)
                    //{
                    //    ViewBag.FeesPaid = payment.Amount;
                    //}
                    //else
                    //{
                    //    ViewBag.FeesPaid = null;
                    //}
                    ViewBag.FeesPaid = "";

                    if (studnt.Class == "PRI 1" || studnt.Class == "PRI 2" || studnt.Class == "PRI 3" || studnt.Class == "PRI 4" || studnt.Class == "PRI 5" || studnt.Class == "PRI 6" || studnt.Class == "JS 1" || studnt.Class == "JS 2" || studnt.Class == "JS 3" || studnt.Class == "SS 1" || studnt.Class == "SS 2" || studnt.Class == "SS 3")
                    {
                        //getting total Cognitive, Affective, Psychomoto, Exam, Scores 
                        ViewBag.TotalCognitive = db.scores.Where(p => p.StudentNumber == StudtNum && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.FirstCA).ToString("0");
                        ViewBag.TotalAffective = db.scores.Where(p => p.StudentNumber == StudtNum && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.SecondCA).ToString("0");
                        ViewBag.TotalPsychomoto = db.scores.Where(p => p.StudentNumber == StudtNum && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.Psychomoto).ToString("0");
                        ViewBag.TotalExam = db.scores.Where(p => p.StudentNumber == StudtNum && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.Exam);
                        ViewBag.TotalScore = db.scores.Where(p => p.StudentNumber == StudtNum && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.Total).ToString("0");
                    }
                    else if (studnt.Class == "NUR 1" || studnt.Class == "NUR 2" || studnt.Class == "NUR 3" || studnt.Class == "PRE-NUR")
                    {
                        ViewBag.Tobtainable = db.scores.Where(p => p.StudentNumber == StudtNum && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.ObtainableScore).ToString("0");
                        ViewBag.Tobtained = db.scores.Where(p => p.StudentNumber == StudtNum && p.AcademicYear == session.AcademicYear && p.Term == term.TermName).Sum(d => d.Exam).ToString("0");
                    }



                    //getting Overall Score for Student
                    score studnt1 = db.scores.FirstOrDefault(u => u.StudentNumber == StudtNum);

                    var x = from d in db.scores
                            where d.StudentNumber == studnt1.StudentNumber && d.AcademicYear == studnt1.AcademicYear && d.Term == term.TermName
                            select d;

                    decimal OverallScores = x.Sum(y => y.Total);
                    var count2 = x.Count();
                    decimal Gradess = OverallScores / count2;

                    //Getting Obtainable Scores
                    ViewBag.ObtainableScore = count2 * 100;

                    if (Gradess >= 80)
                    {
                        ViewBag.OverAllGrades = "A";
                        ViewBag.OverallRating = "EXCELLENT";
                    }
                    else if (Gradess >= 70)
                    {
                        ViewBag.OverAllGrades = "B";
                        ViewBag.OverallRating = "GOOD";
                    }
                    else if (Gradess >= 60)
                    {
                        ViewBag.OverAllGrades = "C";
                        ViewBag.OverallRating = "CREDIT";
                    }
                    else if (Gradess >= 50)
                    {
                        ViewBag.OverAllGrades = "D";
                        ViewBag.OverallRating = "PASS";
                    }
                    else if (Gradess >= 40)
                    {
                        ViewBag.OverAllGrades = "E";
                        ViewBag.OverallRating = "PASS";
                    }
                    else if (Gradess < 40)
                    {
                        ViewBag.OverAllGrades = "F";
                        ViewBag.OverallRating = "FAIL";
                    }


                    //if (term.TermName == "THIRD")
                    //{

                    //    //counting total numbers of subject offerred Per-Term
                    //    var count1st = db.scores.Where(t => t.StudentNumber == StudtNum && t.Term == "FIRST" && t.AcademicYear == session.AcademicYear).Count();
                    //    var count2nd = db.scores.Where(t => t.StudentNumber == StudtNum && t.Term == "SECOND" && t.AcademicYear == session.AcademicYear).Count();
                    //    var count3rd = db.scores.Where(t => t.StudentNumber == StudtNum && t.Term == "THIRD" && t.AcademicYear == session.AcademicYear).Count();

                    //    //calculating cumulative scores for third term
                    //    var TFirst = db.scores.Where(t => t.StudentNumber == StudtNum && t.Term == "FIRST" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);
                    //    var TSecond = db.scores.Where(t => t.StudentNumber == StudtNum && t.Term == "SECOND" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);
                    //    var TThird = db.scores.Where(t => t.StudentNumber == StudtNum && t.Term == "THIRD" && t.AcademicYear == session.AcademicYear).Sum(r => r.Total);

                    //    //calculating overall score for each Term
                    //    var First = TFirst / count1st;
                    //    var Second = TSecond / count2nd;
                    //    var Third = TThird / count3rd;

                    //    //total score for first, Second, Third Term of a Session
                    //    ViewBag.FirstTerm = TFirst.ToString("0");
                    //    ViewBag.SecondTerm = TSecond.ToString("0");
                    //    ViewBag.ThirdTerm = TThird.ToString("0");

                    //    var Total = First + Second + Third;
                    //    var average = Total / 3;
                    //    ViewBag.CAverage = ((TFirst + TSecond + TThird) / 3).ToString("0");

                    //    if (average >= 80)
                    //    {
                    //        ViewBag.OverallGrade = "A";
                    //        ViewBag.OverallRating = "EXCELLENT";
                    //        ViewBag.Statues = "Promoted";

                    //    }
                    //    else if (average >= 70)
                    //    {
                    //        ViewBag.OverallGrade = "B";
                    //        ViewBag.OverallRating = "VERY GOOD";
                    //        ViewBag.Statues = "Promoted";
                    //    }
                    //    else if (average >= 60)
                    //    {
                    //        ViewBag.OverallGrade = "C";
                    //        ViewBag.OverallRating = "GOOD";
                    //        ViewBag.Statues = "Promoted";
                    //    }
                    //    else if (average >= 50)
                    //    {
                    //        ViewBag.OverallGrade = "D";
                    //        ViewBag.OverallRating = "CREDIT";
                    //        ViewBag.Statues = "Promoted on Trial";
                    //    }
                    //    else if (average >= 40)
                    //    {
                    //        ViewBag.OverAllGrades = "E";
                    //        ViewBag.OverallRating = "PASS";
                    //        ViewBag.Statues = "Promoted on Trial";
                    //    }
                    //    else if (average < 40)
                    //    {
                    //        ViewBag.OverAllGrades = "F";
                    //        ViewBag.OverallRating = "FAIL";
                    //        ViewBag.Statues = "Repeat";
                    //    }
                    //}
                    return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == session.AcademicYear && s.Term == term.TermName));
                }
                else
                {
                    ViewBag.NoDetails = "   No result Found, Please enter Apropriate Search";
                    return View(db.scores.ToList().Where(s => s.StudentNumber == "" && s.AcademicYear == "" && s.Term == ""));
                }
            }
            return View(db.scores.ToList().OrderBy(t => t.Subject).Where(s => s.StudentNumber == StudtNum.Trim() && s.AcademicYear == session.AcademicYear && s.Term == term.TermName));
        }


        //Get index for deleting records
        [Authorize(Roles = "Admin")]
        public ActionResult IndexSearchByClassandSubject(string sortOrder, string StudtNumFilter, string Class, string AccademicYearFilter, string AccademicYear, string TermFilter, string Terms, int? page, string Subject)
        {

            //getting dropdown
            ViewBag.AccademicYear = new SelectList(db.Sessions, "Id", "AcademicYear");
            ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");
            ViewBag.Class = new SelectList(db.ClassLists, "Id", "ClassName");
            ViewBag.Subject = new SelectList(db.SchoolSubjectss, "Id", "Subject");

            if (Class != null && AccademicYear != null && Terms != null)
            {
                //getting Term 
                int termID = Convert.ToInt32(Terms);
                Term term = db.terms.Find(termID);

                //getting Session
                int sessionID = Convert.ToInt32(AccademicYear);
                Session session = db.Sessions.Find(sessionID);

                //getting school subjects
                SchoolSubjects subject = db.SchoolSubjectss.Find(Convert.ToInt32(Subject));

                //getting classes
                ClassList classs = db.ClassLists.Find(Convert.ToInt32(Class));

                return View(db.scores.ToList().OrderBy(t => t.Surname).Where(s => s.Class == classs.ClassName && s.AcademicYear == session.AcademicYear && s.Term == term.TermName && s.Subject == subject.Subject));



            }

            return View(db.scores.ToList().Take(0));
        }

        // GET: scores/Details/5
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            score score = db.scores.Find(id);
            Student student = db.Students.FirstOrDefault(t => t.StudentNumber == score.StudentNumber);

            //getting student category
            ViewBag.Category = student.Category;

            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // GET: scores/Create
        [Authorize(Roles = "Teacher, Class Teacher PRE-NUR, Class Teacher NUR 1, Class Teacher NUR 2, Class Teacher NUR 3, Class Teacher PRI 1, Class Teacher PRI 2, Class Teacher PRI 3, Class Teacher PRI 4, Class Teacher PRI 5, Class Teacher PRI 6")]
        public ActionResult Create(string Message)
        {
            ViewBag.Message = Message;
            //getting Teacher Subjects
            Teachers user = db.Teachers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            TeacherSubject teacher = db.TeacherSubjects.Distinct().FirstOrDefault(t => t.TeacherNum == user.StaffNumber);

            //{
            //Finding Student by ID
            //Student student = db.Students.Find(id);
            //if (student != null)
            //ViewBag.StudentNumber = student.StudentNumber;
            //ViewBag.Surname = student.Surname;
            //ViewBag.OtherName = student.OtherName;

            //ViewBag.Category = student.Category;

            //if (student.Category == "Primary" || student.Category == "Senior" | student.Category == "Junior")
            //{
            //    ViewBag.FirstCA = 0;
            //    ViewBag.SecondCA = 0;
            //    ViewBag.Exam = 0;
            //    ViewBag.Psychomoto = 0;
            //}
            //else if (student.Category == "Nursery" || student.Category == "Pre-Nursery")
            //{
            //    ViewBag.Exam = 0;
            //    ViewBag.ObtainableScore = 100;
            //}


            //getting session
            GlobalSettings sesssion = db.GlobalSettings.FirstOrDefault(r => r.Name == "Session");
            ViewBag.AcademicYear = sesssion.Value;

            //getting Term 
            GlobalSettings term = db.GlobalSettings.FirstOrDefault(r => r.Name == "Term");
            ViewBag.Term = term.Value;

            //if (User.IsInRole("Class Teacher PRE-NUR") || User.IsInRole("Class Teacher NUR 1") || User.IsInRole("Class Teacher NUR 2") || User.IsInRole("Class Teacher NUR 3") || User.IsInRole("Class Teacher PRI 1") || User.IsInRole("Class Teacher PRI 2") || User.IsInRole("Class Teacher PRI 3") || User.IsInRole("Class Teacher PRI 4") || User.IsInRole("Class Teacher PRI 5") || User.IsInRole("Class Teacher PRI 6"))
            //{
            //    TeacherSubject Class = db.TeacherSubjects.FirstOrDefault(g => g.TeacherNum == user.StaffNumber);

            //    //if a class teacher of a primary class still teaches another subject in the Secondary class
            //    Subjects findsubj = db.Subjectss.FirstOrDefault(t => t.Class == student.Class);
            //    if (findsubj != null)
            //    {
            //        ViewBag.Subject = new SelectList(db.Subjectss.ToList().Where(e => e.Class == Class.Class), "SubjectName", "SubjectName");
            //        ViewBag.Class = new SelectList(db.Subjectss.Take(1).ToList().Where(e => e.Class == Class.Class), "Class", "Class");
            //    }
            //    else
            //    {
            //        ViewBag.Subject = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber), "Subject", "Subject");
            //        ViewBag.Class = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber), "Class", "Class");
            //    }
            //}


            var listclass = from d in db.TeacherSubjects
                            where d.TeacherNum == user.StaffNumber
                            select d.Class;
            var dst = listclass.Distinct();
            ViewBag.Class = new SelectList(dst).ToList();

            ViewBag.Subject = new SelectList("", "", "");


            //}

            return View();
        }

        // POST: scores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Create([Bind(Include = "Id,StudentNumber,Surname,OtherName,Subject,Term,FirstCA,SecondCA,Exam,Total,AVERAGE,HIGHEST,LOWEST,GRADE,REMARK,PreparedBy,DateRecorded,Class,AcademicYear,Psychomoto,ObtainableScore")] score score)
        {
            //Finding User and their Class  
            Teachers user = db.Teachers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            TeacherSubject teacher = db.TeacherSubjects.FirstOrDefault(t => t.TeacherNum == user.StaffNumber);



            if (User.IsInRole("Class Teacher PRE-NUR") || User.IsInRole("Class Teacher NUR 1") || User.IsInRole("Class Teacher NUR 2") || User.IsInRole("Class Teacher NUR 3") || User.IsInRole("Class Teacher PRI 1") || User.IsInRole("Class Teacher PRI 2") || User.IsInRole("Class Teacher PRI 3") || User.IsInRole("Class Teacher PRI 4") || User.IsInRole("Class Teacher PRI 5") || User.IsInRole("Class Teacher PRI 6"))
            {
                //if a class teacher of a primary class still teaches another subject in the Secondary class
                Subjects findsud = db.Subjectss.FirstOrDefault(t => t.Class == score.Class);


                if (findsud != null)
                {
                    foreach (var Student in db.Students.Where(t => t.Class == score.Class).ToList())
                    {

                        //Ensuring that New Result is not created for a Student Twice
                        score scores = db.scores.FirstOrDefault(s => s.StudentNumber == Student.StudentNumber && s.Subject == score.Subject && s.Term == score.Term && s.AcademicYear == score.AcademicYear);
                        //Searching Student ID
                        var studentId = db.Students.FirstOrDefault(s => s.StudentNumber == score.StudentNumber);
                        int Id = studentId.Id;

                        if (scores == null)
                        {

                            if (studentId.Category == "Nursery" || studentId.Category == "Pre-Nursery")
                            {
                                score.ObtainableScore = 0;
                                score.Exam = 0;
                            }
                            score.StudentNumber = Student.StudentNumber;
                            score.Surname = Student.Surname;
                            score.OtherName = Student.OtherName;
                            score.Class = Student.Class;
                            score.PreparedBy = user.FullName;
                            score.DateRecorded = DateTime.Now;
                            score.Subject = score.Subject;
                            score.ObtainableScore = 100;
                            score.Psychomoto = 0;
                            score.SecondCA = 0;
                            score.FirstCA = 0;
                            score.Exam = 0;
                            score.Total = 0;

                            //Saving Records into Database Table.
                            db.scores.Add(score);
                            db.SaveChanges();



                        }

                        //else
                        //{
                        //    ViewBag.StudentNumber = score.StudentNumber;
                        //    ViewBag.Surname = score.Surname;
                        //    ViewBag.OtherName = score.OtherName;
                        //    ViewBag.Class = score.Class;
                        //    ViewBag.AcademicYear = score.AcademicYear;
                        //    ViewBag.Term = score.Term;
                        //    ViewBag.Subject = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber && e.Class == teacher.Class), "Id", "Subject");


                        //    ViewBag.Message = "Please ensure this RESULT has not been created before";
                        //    return View();

                        //}
                    }
                    string Message = "Result Template for " + score.Subject + " for " + score.Class + ". Was successfully Created. Thank You";
                    return RedirectToAction("Create", new { Message });
                }
                else
                {

                    TeacherSubject tsubject = db.TeacherSubjects.FirstOrDefault(s => s.Subject == score.Subject && s.Class == score.Class);

                    if (tsubject != null)
                    {
                        foreach (var Student in db.Students.Where(t => t.Class == score.Class).ToList())
                        {
                            //Ensuring that New Result is not created for a Student Twice
                            score scores = db.scores.FirstOrDefault(s => s.StudentNumber == Student.StudentNumber && s.Subject == tsubject.Subject && s.Term == score.Term && s.AcademicYear == score.AcademicYear);
                            //Searching Student ID
                            string stdntNum = score.StudentNumber;
                            var studentId = db.Students.FirstOrDefault(s => s.StudentNumber == stdntNum);

                            if (scores == null)
                            {

                                if (studentId.Category == "Nursery" || studentId.Category == "Pre-Nursery")
                                {
                                    score.ObtainableScore = 0;
                                }

                                score.StudentNumber = Student.StudentNumber;
                                score.Surname = Student.Surname;
                                score.OtherName = Student.OtherName;
                                score.Class = Student.Class;
                                score.PreparedBy = user.FullName;
                                score.DateRecorded = DateTime.Now;
                                score.Subject = tsubject.Subject;

                                //Saving Records into Database Table.
                                db.scores.Add(score);
                                db.SaveChanges();



                            }
                        }
                        string Message = "Result Template for " + score.Subject + " for " + score.Class + ". Was successfully Created. Thank You";
                        return RedirectToAction("Create", new { Message });
                    }

                }


            }

            else
            {
                //getting Subject by ID
                TeacherSubject subject = db.TeacherSubjects.FirstOrDefault(s => s.Subject == score.Subject && s.Class == score.Class);


                if (subject != null)
                {
                    foreach (var Student in db.Students.Where(t => t.Class == score.Class).ToList())
                    {

                        //Ensuring that New Result is not created for a Student Twice
                        score scores = db.scores.FirstOrDefault(s => s.StudentNumber == Student.StudentNumber && s.Subject == score.Subject && s.Term == score.Term && s.AcademicYear == score.AcademicYear);

                        if (scores == null)
                        {


                            if (Student.Category == "Nursery" || Student.Category == "Pre-Nursery")
                            {
                                score.ObtainableScore = 0;
                            }
                            score.StudentNumber = Student.StudentNumber;
                            score.Surname = Student.Surname;
                            score.OtherName = Student.OtherName;
                            score.Class = Student.Class;
                            score.PreparedBy = user.FullName;
                            score.DateRecorded = DateTime.Now;
                            score.Subject = subject.Subject;

                            //Saving Records into Database Table.
                            db.scores.Add(score);
                            db.SaveChanges();

                        }
                    }
                    string Message = "Result Template for " + score.Subject + " for " + score.Class + ". Was successfully Created. Thank You";
                    return RedirectToAction("Create", new { Message });
                }
                //else
                //{

                //    //Finding Student by ID
                //    Student student = db.Students.Find(id);
                //    if (student != null)
                //    {
                //        ViewBag.StudentNum = student.StudentNumber;
                //        ViewBag.Surname = student.Surname;
                //        ViewBag.otherName = student.OtherName;
                //        ViewBag.Class = student.Class;
                //        ViewBag.sessionFilter = new SelectList(db.Sessions, "Id", "AcademicYear");
                //        ViewBag.Subject = new SelectList(db.TeacherSubjects.ToList().Where(e => e.TeacherNum == user.StaffNumber && e.Class == teacher.Class), "Id", "Subject");

                //    }
                //    ViewBag.Message = "Please ensure this RESULT has not been created before";
                //    return View();
                //}


            }

            return View();
        }

        // GET: scores/Edit/5
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Edit(int? id, string Clas)
        {
            //Finding Student by ID

            score student = db.scores.Find(id);
            Student stdnt = db.Students.FirstOrDefault(t => t.StudentNumber == student.StudentNumber);
            if (student != null)
            {
                ViewBag.StudentNum = stdnt.StudentNumber;
                ViewBag.Surname = stdnt.Surname;
                ViewBag.otherName = stdnt.OtherName;
                ViewBag.Class = stdnt.Class;
                ViewBag.AcademicYear = student.AcademicYear;
                ViewBag.Term = student.Term;
                ViewBag.Category = stdnt.Category;

            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            score score = db.scores.Find(id);
            if (score == null)
            {
                return HttpNotFound();
            }

            return PartialView(score);
        }

        // POST: scores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Edit([Bind(Include = "Id,StudentNumber,Surname,OtherName,Subject,Class,FirstCA,SecondCA,Exam,Total,AVERAGE,HIGHEST,LOWEST,GRADE,REMARK,PreparedBy,DateRecorded,AcademicYear,Term,Psychomoto,ObtainableScore")] score score)
        {
            //getting Teacher Subjects
            Teachers user = db.Teachers.FirstOrDefault(u => u.UserName == User.Identity.Name);

            //Searching Student ID
            string stdntNum = score.StudentNumber;
            var studentId = db.Students.FirstOrDefault(s => s.StudentNumber == stdntNum);
            int Id = studentId.Id;

            //Calculating total
            decimal CA1 = score.FirstCA;
            decimal CA2 = score.SecondCA;
            decimal CA3 = score.Psychomoto;
            decimal Exam = score.Exam;

            if (Exam > 0)
            {
                decimal total = CA1 + CA2 + CA3 + Exam;
                score.Total = total;

                //calculating Grading and Remarks
                if (total >= 80)
                {
                    score.GRADE = "A";
                    score.REMARK = "EXCELLENT";

                }
                else if (total >= 70)
                {
                    score.GRADE = "B";
                    score.REMARK = "GOOD";
                }
                else if (total >= 60)
                {
                    score.GRADE = "C";
                    score.REMARK = "CREDIT";
                }
                else if (total >= 50)
                {
                    score.GRADE = "D";
                    score.REMARK = "PASS";
                }
                else if (total >= 40)
                {
                    score.GRADE = "E";
                    score.REMARK = "PASS";
                }
                else if (total < 40)
                {
                    score.GRADE = "F";
                    score.REMARK = "FAIL";
                }

            }
            else
            {
                score.Total = CA1 + CA2 + CA3;
                score.GRADE = " ";
                score.REMARK = " ";
            }

            //Saving other Values
            score.DateRecorded = DateTime.Now;
            score.PreparedBy = user.FullName;
            score.AVERAGE = 0;
            score.HIGHEST = 0;
            score.LOWEST = 0;
            score.ObtainableScore = 100;

            db.Entry(score).State = EntityState.Modified;
            db.SaveChanges();

            //getting total numbers of student in class
            int count = db.Students.Count(se => se.Class == score.Class);

            //Calculating Average
            var x = from d in db.scores
                    where d.Subject == score.Subject && d.AcademicYear == score.AcademicYear && d.Term == score.Term && d.Class == score.Class
                    select d;

            decimal ToatalAverage = x.Sum(y => y.Total);


            //getting highest and lowest score
            decimal heighest = x.Max(y => y.Total);
            decimal lowest = x.Min(y => y.Total);

            //Updating Average, Highest and Lowest
            foreach (var item in db.scores.Where(d => d.Subject == score.Subject && d.AcademicYear == score.AcademicYear && d.Class == score.Class && d.Term == score.Term).ToList())
            {

                item.AVERAGE = ToatalAverage / count;
                item.HIGHEST = heighest;
                item.LOWEST = lowest;

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }

            string Category = TempData["Category"].ToString();
            string Message = score.Subject + " score for " + score.StudentNumber + " was successfully recorded";
            return RedirectToAction("IndexUpdate", new { Subject = score.Subject, Class = score.Class, Category = Category, Message });
        }

        // GET: scores/Delete/5
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Admin,Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            score score = db.scores.Find(id);
            Student student = db.Students.FirstOrDefault(t => t.StudentNumber == score.StudentNumber);

            //getting student category
            ViewBag.Category = student.Category;
            if (score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }

        // POST: scores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Class Teacher PRE-NUR,Admin, Class Teacher NUR 1,Class Teacher NUR 2,Class Teacher NUR 3,Class Teacher PRI 1,Class Teacher PRI 2,Class Teacher PRI 3,Class Teacher PRI 4,Class Teacher PRI 5,Class Teacher PRI 6")]
        public ActionResult DeleteConfirmed(int id)
        {

            score score = db.scores.Find(id);

            string studntNumb = score.StudentNumber;
            Student students = db.Students.FirstOrDefault(s => s.StudentNumber == studntNumb);

            db.scores.Remove(score);
            db.SaveChanges();
            if (User.IsInRole("Admin"))
            {

                return RedirectToAction("IndexDelete");
            }
            else
            {
                int Id = students.Id;

                return RedirectToAction("IndexUpdate", new { Id, });
            }

        }

        public ActionResult Verify(string Term, string AddNo, string AcademicYear)
        {
            //updating HOD Result verification statues
            foreach (var item in db.scores.Where(d => d.AcademicYear == AcademicYear && d.StudentNumber == AddNo && d.Term == Term).ToList())
            {
                item.HODVerify = "Verified";

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }

            string AccademicYear = TempData["Session"].ToString();
            string Terms = TempData["Term"].ToString();
            string StudtNum = TempData["No"].ToString();

            string Message = "The Result of the student with Admission Number: " + AddNo + " " + "Term: " + Term + " " + "Session: " + AcademicYear + " has been verified";
            return RedirectToAction("IndexHod", new { Message, StudtNum, Terms, AccademicYear });
        }

        //cummulative score for student based on class and subject
        public ActionResult CommulativeScore(string Subject, string Class, string Term, string AccademicYear)
        {
            //if (Subject != "" && Class != "" && Term != "" && AccademicYear != "")
            //{
            //    return View(db.scores.ToList().OrderBy(t => t.StudentNumber).OrderBy(t => t.Subject).Where(t=>t.Subject==Subject&&t.Class==Class&&t.Term==Term&&t.AcademicYear==AccademicYear));
            //}
            return View(db.scores.ToList().OrderBy(t => t.StudentNumber)/*.OrderBy(t => t.Subject)*/);
        }


        //check Result
        public ActionResult CheckResult(string phone)
        {
            //gettting student numbers
            Student student = db.Students.FirstOrDefault(t => t.PhoneNumber == phone);
            if (student != null)
            {
                ViewBag.Message = "Please select the Learner's Admission Number and Academic Term, before clicking the check Result button. ";

                ViewBag.StudentNumber = new SelectList(db.Students.Where(t => t.PhoneNumber == phone), "Id", "StudentNumber");

                //getting current academic year from global settings 
                GlobalSettings academicyear = db.GlobalSettings.FirstOrDefault(e => e.Name == "Session");
                ViewBag.AccademicYear = academicyear.Value;

                ViewBag.Terms = new SelectList(db.terms, "Id", "TermName");
                ViewBag.Phone = phone;
            }
            else
            {

                ViewBag.Message = "No student Number is Associated with this Phone Number, Please Ensure the right Phone Number is enetered.";

                ViewBag.StudentNumber = new SelectList(db.Students.Where(t => t.PhoneNumber == phone), "StudentNumber", "StudentNumber");
                ViewBag.AccademicYear = new SelectList(db.Sessions, "AcademicYear", "AcademicYear");
                ViewBag.Terms = new SelectList(db.terms, "TermName", "TermName");
                ViewBag.Phone = phone;
            }



            return View();
        }


        [HttpGet]
        public JsonResult GetNo(string ClassId, string Class)
        {


            if (ClassId != null)
            {
                ViewBag.RegNo = db.Students.Where(a => a.Class.Equals(ClassId)).OrderBy(a => a.StudentNumber).ToList();

                if (Request.IsAjaxRequest())
                {
                    return new JsonResult
                    {
                        Data = ViewBag.RegNo,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }

            }
            else if (Class != null)
            {
                Teachers user = db.Teachers.FirstOrDefault(t => t.UserName == User.Identity.Name);

                var listSub = from d in db.TeacherSubjects
                              where d.TeacherNum == user.StaffNumber && d.Class == Class
                              select d.Subject;
                var sub = listSub.Distinct();
                ViewBag.Subject = sub.ToList();

                if (Request.IsAjaxRequest())
                {
                    return new JsonResult
                    {
                        Data = ViewBag.Subject,
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
