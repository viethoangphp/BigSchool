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
            List<Course> list = db.Courses.Where(m=>m.Status ==1).ToList();
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
                model.Status = 1;
                db.Courses.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "Course");
            }
            Course course = new Course();
            course.ListCategory = db.Categories.ToList();
            return View("Create", course);
        }
        [HttpPost]
        public JsonResult Attending(int id)
        {
            DBContext db = new DBContext();
            var userID = User.Identity.GetUserId();
            if(db.Attendances.Where(m=>m.UserId == userID && m.CourseId == id).FirstOrDefault() == null)
            {
                Attendance attendance = new Attendance();
                attendance.CourseId = id;
                attendance.UserId = userID;
                db.Attendances.Add(attendance);
                db.SaveChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                db.Attendances.Remove(db.Attendances.Where(m => m.UserId == userID && m.CourseId == id).FirstOrDefault());
                db.SaveChanges();
                return Json("false", JsonRequestBehavior.AllowGet);
            }
           
        }
        public ActionResult Update(int id)
        {
            var course = new DBContext().Courses.Find(id);
            course.ListCategory = new DBContext().Categories.ToList();
            return View(course);
        }
        [HttpPost]
        public ActionResult Update(Course model)
        {
            DBContext db = new DBContext();
            var course = db.Courses.Find(model.Id);
            ModelState.Remove("LectureId");
            if (ModelState.IsValid)
            {
                course.Place = model.Place;
                course.DateTime = model.DateTime;
                course.CategoryId = model.CategoryId;
                db.SaveChanges();
                return RedirectToAction("Index", "Course");
            }
            course.ListCategory = new DBContext().Categories.ToList();
            return View(course);
        }
        public ActionResult Delete(int id)
        {
            DBContext db = new DBContext();
            var course = db.Courses.Find(id);
            if(course != null)
            {
                course.Status = 0;
                db.SaveChanges();
                return RedirectToAction("Index", "Course");
            }
            return HttpNotFound();
        }
        public JsonResult Follow(string followee)
        {
            DBContext db = new DBContext();
            var follower = User.Identity.GetUserId();
            if(new CheckAttendace().isFollow(follower,followee) == true)
            {
                Following item = new Following();
                item.FolloweeId = followee;
                item.FollowerId = follower;
                db.Followings.Add(item);
                db.SaveChanges();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            else
            {
                Following item = db.Followings.Where(m => m.FolloweeId == followee && m.FollowerId == follower).FirstOrDefault();
                db.Followings.Remove(item);
                db.SaveChanges();
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
       
    }
}