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
    [Authorize]
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            DBContext db = new DBContext();
            List<Course> list = db.Courses.ToList();
            foreach(var item in list)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(item.LectureId);
                item.name = user.name;
            }    
            return View(list);
        }
        public ActionResult Create()
        {
            DBContext db = new DBContext();
            Course course = new Course();
            course.ListCategory = db.Categories.ToList();
            return View(course);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course model)
        {
            DBContext db = new DBContext();
            ModelState.Remove("LectureId");
            if (ModelState.IsValid)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                model.LectureId = user.Id;
                db.Courses.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "Course");
            }
         
            Course course = new Course();
            course.ListCategory = db.Categories.ToList();
            return View("Create", course);
        }
    }
}