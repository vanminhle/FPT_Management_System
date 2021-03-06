using FPT_Management_System.Models;
using FPT_Management_System.Utils;
using FPT_Management_System.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FPT_Management_System.Controllers
{
    [Authorize(Roles = Role.Staff)]
    public class CoursesController : Controller
    {
        public ApplicationDbContext _context;

        public CoursesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult CourseIndex(string searchCourses)
        {
            var courses = _context.Courses
                .Include(t => t.CourseCategory)
                .ToList();

            if (!string.IsNullOrEmpty(searchCourses))
            {
                courses = courses
                    .Where(t => t.Name.ToLower().Contains(searchCourses.ToLower())
                            || t.CourseCategory.Name.ToLower().Contains(searchCourses.ToLower())
                ).ToList();
            }

            return View(courses);
        }

        [HttpGet]
        public ActionResult CourseCreate()
        {
            var categories = _context.CourseCategories.ToList();
            var viewModel = new CourseCategoriesViewModels()
            {
                CourseCategories = categories
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CourseCreate(CourseCategoriesViewModels model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CourseCategoriesViewModels
                {
                    Courses = model.Courses,
                    CourseCategories = _context.CourseCategories.ToList()
                };
                return View(viewModel);
            }

            var isExists = _context.Courses.Any(c => c.Name.Contains(model.Courses.Name));
            if (isExists)
            {
                TempData["error"] = "Course is already exists";
                return RedirectToAction("CourseIndex", "Courses");
            }
            var newCourse = new Course()
            {
                Name = model.Courses.Name,
                Description = model.Courses.Description,
                CategoryId = model.Courses.CategoryId,
            };

            _context.Courses.Add(newCourse);
            _context.SaveChanges();

            TempData["message"] = "Course create successfully";
            return RedirectToAction("CourseIndex", "Courses");
        }

        [HttpGet]
        public ActionResult CourseDelete(int id)
        {
            var courseInDb = _context.Courses
                .SingleOrDefault(t => t.CourseID == id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }

            _context.Courses.Remove(courseInDb);
            _context.SaveChanges();

            TempData["message"] = "Course information is successfully deleted";
            return RedirectToAction("CourseIndex", "Courses");
        }

        [HttpGet]
        public ActionResult CourseEdit(int id)
        {
            var courseInDb = _context.Courses
                .SingleOrDefault(t => t.CourseID == id);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CourseCategoriesViewModels
            {
                Courses = courseInDb,
                CourseCategories = _context.CourseCategories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CourseEdit(CourseCategoriesViewModels model)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound();
            }

            var isExists = _context.Courses.Any
                (c => c.Name.Equals(model.Courses.Name));
            if (isExists)
            {
                TempData["error"] = "Course is already exists";
                return RedirectToAction("CourseIndex", "Courses");
            }

            var courseInDb = _context.Courses
                .SingleOrDefault(t => t.CourseID == model.Courses.CourseID);
            if (courseInDb == null)
            {
                return HttpNotFound();
            }

            courseInDb.Name = model.Courses.Name;
            courseInDb.Description = model.Courses.Description;
            courseInDb.CategoryId = model.Courses.CategoryId;

            _context.SaveChanges();
            TempData["message"] = "Course information is successfully changed";
            return RedirectToAction("CourseIndex", "Courses");
        }
    }
}