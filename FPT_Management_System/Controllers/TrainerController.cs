using FPT_Management_System.Models;
using FPT_Management_System.Utils;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;

namespace FPT_Management_System.Controllers
{
    [Authorize(Roles = Role.Trainer)]
    public class TrainerController : Controller
    {
        private ApplicationDbContext _context;

        public TrainerController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult TrainerProfile()
        {
            var userId = User.Identity.GetUserId();
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == userId);
            return View(trainerInDb);
        }

        [HttpGet]
        public ActionResult EditTrainerAccount(string id)
        {
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == id);
            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            return View(trainerInDb);
        }

        [HttpPost]
        public ActionResult EditTrainerAccount(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }
            var trainerInDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == trainer.TrainerId);

            if (trainerInDb == null)
            {
                return HttpNotFound();
            }
            trainerInDb.FullName = trainer.FullName;
            trainerInDb.Age = trainer.Age;
            trainerInDb.Address = trainer.Address;
            trainerInDb.Specialty = trainer.Specialty;

            _context.SaveChanges();

            TempData["Message"] = "Staff Account has Successfully Create";
            return RedirectToAction("TrainerProfile", "Trainer");
        }

        [HttpGet]
        public ActionResult GetCourses()
        {
            var courseCategory = _context.CourseCategories.ToList();
            var trainer = _context.Trainers.ToList();
            var userId = User.Identity.GetUserId();
            var getCourses = _context.TrainersToCourses
                .Where(t => t.Trainer.TrainerId == userId)
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