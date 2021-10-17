using FPT_Management_System.Models;
using FPT_Management_System.Utils;
using FPT_Management_System.ViewModels;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
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

            return RedirectToAction("TrainerProfile", "Trainer");
        }

        [HttpGet]
        public ActionResult GetTrainerCourses()
        {
            var userId = User.Identity.GetUserId();

            var getCourses = _context.Courses.ToList();
            var getTrainee = _context.Trainees.ToList();

            var getTrainerCourses = _context.TrainersToCourses
                .Where(t => t.Trainer.TrainerId == userId)
                .Select(c => c.CourseId)
                .ToList();

            List<TraineeInCourseViewModels> traineeInCourses = new List<TraineeInCourseViewModels>();

            foreach (var courseId in getTrainerCourses)
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