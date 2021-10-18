using FPT_Management_System.Models;
using FPT_Management_System.Utils;
using Microsoft.AspNet.Identity;
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
        public ActionResult GetCourses()
        {
            var courseCategory = _context.CourseCategories.ToList();
            var userId = User.Identity.GetUserId();
            var getCourses = _context.TraineesToCourses
                .Where(t => t.Trainee.TraineeId == userId)
                .Select(c => c.Course)
                .ToList();
            return View(getCourses);
        }

        [HttpGet]
        public ActionResult GetTraineeInCourses(int id)
        {
            var getTraineeInCourses = _context.TraineesToCourses
                .Where(t => t.CourseId == id)
                .Select(t => t.Trainee)
                .ToList();

            return View(getTraineeInCourses);
        }
    }
}