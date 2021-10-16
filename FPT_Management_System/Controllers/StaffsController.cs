using FPT_Management_System.Models;
using FPT_Management_System.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FPT_Management_System.Controllers
{
    public class StaffsController : Controller
    {
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;

        public StaffsController()
        {
            _context = new ApplicationDbContext();
        }

        public StaffsController(ApplicationUserManager userManager)
        {
            _context = new ApplicationDbContext();
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize(Roles = "staff")]
        public ActionResult IndexTrainee()
        {
            var trainee = _context.Trainees.ToList();
            var user = _context.Users.ToList();

            return View(trainee);
        }

        [Authorize(Roles = "staff")]
        [HttpGet]
        public ActionResult CreateTraineeAccount()
        {
            return View();
        }

        [Authorize(Roles = "staff")]
        [HttpPost]
        public async Task<ActionResult> CreateTraineeAccount(TraineeAccountViewModels viewModel)
        {
            if (!ModelState.IsValid)
            {
                var user = new ApplicationUser
                { UserName = viewModel.RegisterViewModels.Email, Email = viewModel.RegisterViewModels.Email };
                var result = await UserManager.CreateAsync(user, viewModel.RegisterViewModels.Password);
                var traineeId = user.Id;
                var newTrainee = new Trainee()
                {
                    TraineeId = traineeId,
                    FullName = viewModel.Trainees.FullName,
                    Age = viewModel.Trainees.Age,
                    DateOfBirth = viewModel.Trainees.DateOfBirth,
                    Education = viewModel.Trainees.Education
                };

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "trainee");
                    _context.Trainees.Add(newTrainee);
                    _context.SaveChanges();
                }
                AddErrors(result);
            }

            return RedirectToAction("IndexTrainee", "Staffs");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [Authorize(Roles = "staff")]
        [HttpGet]
        public ActionResult EditTraineeAccount(string id)
        {
            var traineeInDb = _context.Trainees.SingleOrDefault(u => u.TraineeId == id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            return View(traineeInDb);
        }

        [Authorize(Roles = "staff")]
        [HttpPost]
        public ActionResult EditTraineeAccount(Trainee trainee)
        {
            if (!ModelState.IsValid)
            {
                return View(trainee);
            }
            var traineeInfoInDb = _context.Trainees.SingleOrDefault(t => t.TraineeId == trainee.TraineeId);

            if (traineeInfoInDb == null)
            {
                return HttpNotFound();
            }
            traineeInfoInDb.FullName = trainee.FullName;
            traineeInfoInDb.Age = trainee.Age;
            traineeInfoInDb.DateOfBirth = trainee.DateOfBirth;
            traineeInfoInDb.Education = trainee.Education;
            _context.SaveChanges();

            return RedirectToAction("IndexTrainee", "Staffs");
        }

        [Authorize(Roles = "staff")]
        [HttpGet]
        public ActionResult DeleteTraineeAccount(string id)
        {
            var traineeInDb = _context.Users.SingleOrDefault(i => i.Id == id);
            var traineeInfoInDb = _context.Trainees.SingleOrDefault(i => i.TraineeId == id);
            if (traineeInDb == null || traineeInfoInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(traineeInDb);
            _context.Trainees.Remove(traineeInfoInDb);
            _context.SaveChanges();
            return RedirectToAction("IndexTrainee", "Staffs");
        }

        [Authorize(Roles = "staff")]
        [HttpGet]
        public ActionResult TraineeInfoDetails(string id)
        {
            var traineeId = User.Identity.GetUserId();

            var traineeInfoInDb = _context.Trainees
                .SingleOrDefault(t => t.TraineeId == id);

            if (traineeInfoInDb == null)
            {
                return HttpNotFound();
            }
            return View(traineeInfoInDb);
        }

        [Authorize(Roles = "staff")]
        public ActionResult TraineePasswordReset(string id)
        {
            var traineeInDb = _context.Users.SingleOrDefault(i => i.Id == id);
            if (traineeInDb == null)
            {
                return HttpNotFound();
            }
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            userId = traineeInDb.Id;
            if (userId != null)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                userManager.RemovePassword(userId);
                string newPassword = "DefaultPassword@123";
                userManager.AddPassword(userId, newPassword);
            }
            _context.SaveChanges();
            return RedirectToAction("IndexTrainee", "Staffs");
        }
    }
}