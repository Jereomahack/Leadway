using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using LightWay.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LightWay.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string RoleName { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
        public System.Data.Entity.DbSet<LightWay.Models.Teachers> Teachers { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.TeacherSubject> TeacherSubjects { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Subjects> Subjectss { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Grades> Grades { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.ClassList> ClassLists { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.GlobalSettings> GlobalSettings { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Payment> payments { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.score> scores { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Session> Sessions { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.SchoolSubjects> SchoolSubjectss { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Expenditure> Expenditures { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.HOD> Hod { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.PaymentType> PaymentTypes { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Transactions> Transactionss { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Emailing> Emailings { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Transport> Transports { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Route> Routes { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.TransportMember> TransportMembers { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.StaffPayroll> StaffPayrolls { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Hostel> Hostels { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.HostelMember> HostelMembers { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Vendor> Vendors { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Products> Products { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Inventory> Inventories { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Distribution> Distributions { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Store> Stores { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.TransactionInventory> TransactionInventories { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.ParentCode> ParentCodes { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Loan> Loans { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.LoanTransact> LoanTransacts { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Deduction> Deductions { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.DeductionType> DeductionTypes { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Bonus> Bonus { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.BonusType> BonusTypes { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Term> terms { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.StudentTransistion> StudentTransistions { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.CodeGenerator> CodeGenerators { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.StudentOldNumbers> OldAdmissionNumbers { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.PaymentCategory> PaymentCategories { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.PaymentSubCategory> PaymentSubCategories { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.Fee> Fees { get; set; }
        public System.Data.Entity.DbSet<LightWay.Models.Admission> admissions { get; set; }
        public System.Data.Entity.DbSet<LightWay.Models.State> state { get; set; }
        public System.Data.Entity.DbSet<LightWay.Models.LGA> lgas { get; set; }
        public System.Data.Entity.DbSet<LightWay.Models.ExpType> exptypes { get; set; }

        public System.Data.Entity.DbSet<LightWay.Models.SchoolLogo> SchoolLogoes { get; set; }
    }


}
