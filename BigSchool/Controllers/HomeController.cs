using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            DBContext db = new DBContext();
            List<Course> list = db.Courses.Where(m=>m.Status ==1).ToList();
            foreach (var item in list)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.LectureId);
                item.name = user.name;
            }
            return View(list);

        }

    }
}
