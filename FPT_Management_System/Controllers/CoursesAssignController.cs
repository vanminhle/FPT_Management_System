using FPT_Management_System.Models;
using FPT_Management_System.Utils;
using FPT_Management_System.ViewModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FPT_Management_System.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class CoursesAssignController : Controller
    {
        private ApplicationDbContext _context;

        public CoursesAssignController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult GetTrainers(string searchString)
        {
            var courses = _context.Courses.Include(t => t.CourseCategory).ToList();
            var trainer = _context.TrainersToCourses.ToList();

            List<TrainerInCourseViewModels> viewModel = _context.TrainersToCourses
                .GroupBy(i => i.Course)
                .Select(res => new TrainerInCourseViewModels
                {
                    Course = res.Key,
                    Trainers = res.Select(u => u.Trainer).ToList()
                })
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                viewModel = viewModel
                    .Where(t => t.Course.Name.ToLower().Contains(searchString.ToLower()))
                    .ToList();
            }
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddTrainer()
        {
            var viewModel = new TrainersToCoursesViewModels
            {
                Courses = _context.Courses.ToList(),
                Trainers = _context.Trainers.ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddTrainer(TrainersToCoursesViewModels model)
        {
            //goi lai index
            var getViewModel = new TrainersToCoursesViewModels
            {
                Courses = _context.Courses.ToList(),
                Trainers = _context.Trainers.ToList(),
            };

            if (!ModelState.IsValid)
            {
                return View(getViewModel);
            }

            var viewModel = new TrainersToCourse
            {
                CourseId = model.CourseId,
                TrainerId = model.TrainerId
            };

            //check is exist
            List<TrainersToCourse> traineerstoCourses = _context.TrainersToCourses.ToList();
            bool alreadyExist = traineerstoCourses
                .Any(item => item.CourseId == model.CourseId && item.TrainerId == model.TrainerId);
            if (alreadyExist == true)
            {
                ModelState.AddModelError("", "Trainer is Already Exist");
                return View(getViewModel);
            }

            _context.TrainersToCourses.Add(viewModel);
            _context.SaveChanges();
            TempData["success"] = "Add Trainer to course successfully";
            return RedirectToAction("GetTrainers", "CoursesAssign");
        }

        [HttpGet]
        public ActionResult RemoveTrainer()
        {
            var getTrainer = _context.TrainersToCourses.Select(t => t.Trainer)
                .Distinct()
                .ToList();
            var getCourse = _context.TrainersToCourses.Select(t => t.Course)
                .Distinct()
                .ToList();
            var viewModel = new TrainersToCoursesViewModels
            {
                Courses = getCourse,
                Trainers = getTrainer,
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RemoveTrainer(TrainersToCoursesViewModels model)
        {
            var getTrainer = _context.TrainersToCourses
                .SingleOrDefault(c => c.CourseId == model.CourseId && c.TrainerId == model.TrainerId);
            if (getTrainer == null)
            {
                TempData["message"] = "That Trainee is not in this course";
                return RedirectToAction("GetTrainers", "CoursesAssign");
            }

            _context.TrainersToCourses.Remove(getTrainer);
            _context.SaveChanges();

            TempData["message"] = "Trainer remove successfully";
            return RedirectToAction("GetTrainers", "CoursesAssign");
        }

        //Trainees

        [HttpGet]
        public ActionResult GetTrainees(string searchString)
        {
            var courses = _context.Courses.Include(t => t.CourseCategory).ToList();
            var trainees = _context.TraineesToCourses.ToList();

            List<TraineeInCourseViewModels> viewModel = _context.TraineesToCourses
                .GroupBy(i => i.Course)
                .Select(res => new TraineeInCourseViewModels
                {
                    Course = res.Key,
                    Trainees = res.Select(u => u.Trainee).ToList(),
                })
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                viewModel = viewModel
                    .Where(t => t.Course.Name
                    .ToLower()
                    .Contains(searchString.ToLower()))
                    .ToList();
            }

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult AddTrainee()
        {
            var viewModel = new TraineesToCoursesViewModels
            {
                Courses = _context.Courses.ToList(),
                Trainees = _context.Trainees.ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddTrainee(TraineesToCoursesViewModels model)
        {
            //get lai view neu model invalid
            var getViewModel = new TraineesToCoursesViewModels
            {
                Courses = _context.Courses.ToList(),
                Trainees = _context.Trainees.ToList(),
            };
            if (!ModelState.IsValid)
            {
                return View(getViewModel);
            }
            var viewModel = new TraineesToCourse
            {
                CourseId = model.CourseId,
                TraineeId = model.TraineeId
            };

            List<TraineesToCourse> traineestoCourses = _context.TraineesToCourses.ToList();
            bool alreadyExist = traineestoCourses
                .Any(item => item.CourseId == model.CourseId && item.TraineeId == model.TraineeId);
            if (alreadyExist is true)
            {
                ModelState.AddModelError("", "Trainee is Already Exist");
                return View(getViewModel);
            }
            else
            {
                _context.TraineesToCourses.Add(viewModel);
                _context.SaveChanges();
            }

            TempData["success"] = "Add Trainee to course successfully";
            return RedirectToAction("GetTrainees", "CoursesAssign");
        }

        [HttpGet]
        public ActionResult RemoveTrainee()
        {
            var getTrainee = _context.TraineesToCourses.Select(t => t.Trainee)
                .Distinct()
                .ToList();
            var getCourse = _context.TraineesToCourses.Select(t => t.Course)
                .Distinct()
                .ToList();
            var viewModel = new TraineesToCoursesViewModels
            {
                Courses = getCourse,
                Trainees = getTrainee,
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RemoveTrainee(TraineesToCoursesViewModels model)
        {
            var getTrainee = _context.TraineesToCourses
                .SingleOrDefault(c => c.CourseId == model.CourseId && c.TraineeId == model.TraineeId);
            if (getTrainee == null)
            {
                TempData["message"] = "That Trainee is not in this course";
                return RedirectToAction("GetTrainees", "CoursesAssign");
            }

            _context.TraineesToCourses.Remove(getTrainee);
            _context.SaveChanges();

            TempData["message"] = "Trainee remove successfully";
            return RedirectToAction("GetTrainees", "CoursesAssign");
        }
    }
}