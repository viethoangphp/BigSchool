using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
namespace BigSchool.Models
{
    public class CheckAttendace
    {
        public bool isAttendace(string userId, int courseId)
        {
            DBContext db = new DBContext();
            Attendance item = db.Attendances.Where(m => m.CourseId == courseId && m.UserId == userId).FirstOrDefault();
            if (item == null)
                return true;
            return false;
        }
        public bool isFollow(string followerId, string followeeId)
        {
            // follower là người đang login
            // followee là người đc theo dõi bỏi thằng đang đăng login 
            DBContext db = new DBContext();
            Following fl = db.Followings.Where(m => m.FolloweeId == followeeId && m.FollowerId == followerId).FirstOrDefault();
            if (fl == null)
                return true;
            return false;
        }
    }
}