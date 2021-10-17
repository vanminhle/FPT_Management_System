using FPT_Management_System.Models;
using FPT_Management_System.Utils;
using FPT_Management_System.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FPT_Management_System.Controllers
{
    [Authorize(Roles = Role.Staff)]
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

        public ActionResult IndexTrainee(string searchTrainee)
        {
            var trainee = _context.Trainees.ToList();
            var user = _context.Users.ToList();

            if (!String.IsNullOrEmpty(searchTrainee))
            {
                trainee = trainee
                    .FindAll(s => s.FullName.ToLower().Contains(searchTrainee.ToLower())
                            || s.Age.ToString().Contains(searchTrainee)
                ).ToList();
            }

            return View(trainee);
        }

        [HttpGet]
        public ActionResult CreateTraineeAccount()
        {
            return View();
        }

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