using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LightWay.Models;
using System.Web.Mvc;

namespace LightWay.Controllers
{
    public class BaseController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public BaseController()
        {

            var user = System.Threading.Thread.CurrentPrincipal;
            //get user role from db users
            if (user.Identity.Name != "")
            {
                ApplicationUser getuser = db.Users.FirstOrDefault(t => t.UserName == user.Identity.Name);
                ViewBag.Role = getuser.RoleName;
            }


            //GET school name
            GlobalSettings sch = db.GlobalSettings.FirstOrDefault(t => t.Name == "School");
            ViewBag.School = sch.Value;

            //GET school Address
            GlobalSettings Address = db.GlobalSettings.FirstOrDefault(t => t.Name == "Address");
            ViewBag.Address = Address.Value;

            //GET other detail of the school
            GlobalSettings others = db.GlobalSettings.FirstOrDefault(t => t.Name == "Other Details");
            ViewBag.Others = others.Value;

            //GET other detail of the school
            GlobalSettings href = db.GlobalSettings.FirstOrDefault(t => t.Name == "Url");
            ViewBag.Href = href.Value;
        }
    }
}