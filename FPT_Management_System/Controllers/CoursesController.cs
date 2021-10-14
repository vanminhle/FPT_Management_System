using FPT_Management_System.Models;
using FPT_Management_System.Utils;
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
        public ActionResult Index(string searchString)
        {
            var courses = _context.Courses
                .Include(t => t.CourseCategory)
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
                courses = courses.Where(t => t.Description.ToLower().Contains(searchString.ToLower())
#pragma warning restore RCS1155 // Use StringComparison when comparing strings.
#pragma warning disable RCS1155 // Use StringComparison when comparing strings.
                            || t.CourseCategory.Description.ToLower().Contains(searchString.ToLower())
#pragma warning restore RCS1155 // Use StringComparison when comparing strings.
                ).ToList();
            }

            return View(courses);
        }
    }
}