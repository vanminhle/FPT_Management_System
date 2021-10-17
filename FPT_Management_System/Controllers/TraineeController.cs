using FPT_Management_System.Models;
using FPT_Management_System.Utils;
using FPT_Management_System.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FPT_Management_System.Controllers
{
    [Authorize(Roles = Role.Trainee)]
    public class TraineeController : Controller
    {
        private ApplicationDbContext _context;

        public TraineeController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult TraineeProfile()
        {
            var userId = User.Identity.GetUserId();
            var traineeInDb = _context.Trainees.SingleOrDefault(t => t.TraineeId == userId);
            return View(traineeInDb);
        }

        [HttpGet]
        public ActionResult GetTraineeCourses()
        {
            var userId = User.Identity.GetUserId();
            var getTraineeCourses = _context.TraineesToCourses
                .Where(t => t.Trainee.TraineeId == userId)
                .Select(c => c.CourseId)
                .ToList();

            List<TraineeInCourseViewModels> traineeInCourses = new List<TraineeInCourseViewModels>();

            foreach (var courseId in getTraineeCourses)
            {
                var trainees = _context.TraineesToCourses
                .Where(t => t.CourseId == courseId)
                .GroupBy(c => c.Course)
                .Select(res => new TraineeInCourseViewModels
                {
                    Course = res.Key,
                    Trainees = res.Select(u => u.Trainee).ToList()
                })
                .ToList();
                traineeInCourses.AddRange(trainees);
            }

            return View(traineeInCourses);
        }
    }
}