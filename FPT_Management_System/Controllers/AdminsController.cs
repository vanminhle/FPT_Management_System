using FPT_Management_System.Models;
using FPT_Management_System.Utils;
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
    [Authorize(Roles = Role.Admin)]
    public class AdminsController : Controller
    {
        //tao ket noi
        private ApplicationDbContext _context;

        private ApplicationUserManager _userManager;

        private ApplicationSignInManager _signInManager;

        public AdminsController()
        {
            _context = new ApplicationDbContext();
        }

        public AdminsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _context = new ApplicationDbContext();
            UserManager = userManager;
            SignInManager = signInManager;
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

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        [HttpGet]
        public ActionResult IndexStaff()
        {
            var staff = _context.Staffs.ToList();
            var user = _context.Users.ToList();

            return View(staff);
        }

        [HttpGet]
        public ActionResult CreateStaffAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateStaffAccount(StaffAccountViewModels viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                { UserName = viewModel.RegisterViewModels.Email, Email = viewModel.RegisterViewModels.Email };
                var result = await UserManager.CreateAsync(user, viewModel.RegisterViewModels.Password);
                var staffId = user.Id;

                var newStaff = new Staff()
                {
                    StaffId = staffId,
                    FullName = viewModel.Staffs.FullName,
                    Age = viewModel.Staffs.Age,
                    Address = viewModel.Staffs.Address,
                };

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "staff");
                    _context.Staffs.Add(newStaff);
                    _context.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Staff Account is Already Create";
                    return RedirectToAction("IndexStaff", "Admins");
                }
                AddErrors(result);
            }
            TempData["message"] = "Staff Account has Successfully Create";
            return RedirectToAction("IndexStaff", "Admins");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpGet]
        public ActionResult EditStaffAccount(string id)
        {
            var staffInDb = _context.Staffs.SingleOrDefault(u => u.StaffId == id);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }
            return View(staffInDb);
        }

        [HttpPost]
        public ActionResult EditStaffAccount(Staff staff)
        {
            if (!ModelState.IsValid)
            {
                return View(staff);
            }
            var staffInfoInDb = _context.Staffs.SingleOrDefault(t => t.StaffId == staff.StaffId);

            if (staffInfoInDb == null)
            {
                return HttpNotFound();
            }
            staffInfoInDb.FullName = staff.FullName;
            staffInfoInDb.Age = staff.Age;
            staffInfoInDb.Address = staff.Address;
            _context.SaveChanges();
            TempData["message"] = "Staff Info has Successfully Changed";
            return RedirectToAction("IndexStaff", "Admins");
        }

        [HttpGet]
        public ActionResult DeleteStaffAccount(string id)
        {
            var staffInDb = _context.Users.SingleOrDefault(i => i.Id == id);
            var staffInfoInDb = _context.Staffs.SingleOrDefault(i => i.StaffId == id);
            if (staffInDb == null || staffInfoInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(staffInDb);
            _context.Staffs.Remove(staffInfoInDb);
            _context.SaveChanges();
            TempData["message"] = "Staff Account has Successfully Deleted";
            return RedirectToAction("IndexStaff", "Admins");
        }

        [HttpGet]
        public ActionResult StaffInfoDetails(string id)
        {
            var staffInfoInDb = _context.Staffs
                .SingleOrDefault(t => t.StaffId == id);

            if (staffInfoInDb == null)
            {
                return HttpNotFound();
            }
            return View(staffInfoInDb);
        }

        [HttpGet]
        public ActionResult StaffPasswordChange()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StaffPasswordChange(ChangePasswordViewModels viewModel, string id)
        {
            var userInDb = _context.Users.SingleOrDefault(i => i.Id == id);
            if (userInDb == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();

            if (userInDb == null)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                userManager.RemovePassword(userId);
                string newPassword = viewModel.NewPassword;
                userManager.AddPassword(userId, newPassword);
            }
            _context.SaveChanges();
            TempData["message"] = "Staff password has Successfully changed";
            return RedirectToAction("IndexStaff", "Admins");
        }

        //FOR TRAINER

        [HttpGet]
        public ActionResult IndexTrainer()
        {
            var trainer = _context.Trainers.ToList();
            var user = _context.Users.ToList();

            return View(trainer);
        }

        [HttpGet]
        public ActionResult CreateTrainerAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateTrainerAccount(TrainerAccountViewModels viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                { UserName = viewModel.RegisterViewModels.Email, Email = viewModel.RegisterViewModels.Email };
                var result = await UserManager.CreateAsync(user, viewModel.RegisterViewModels.Password);
                var trainerId = user.Id;
                var newTrainer = new Trainer()
                {
                    TrainerId = trainerId,
                    FullName = viewModel.Trainers.FullName,
                    Age = viewModel.Trainers.Age,
                    Address = viewModel.Trainers.Address,
                    Specialty = viewModel.Trainers.Specialty
                };

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "trainer");
                    _context.Trainers.Add(newTrainer);
                    _context.SaveChanges();
                }
                else
                {
                    TempData["error"] = "Trainer Account is Already Create";
                    return RedirectToAction("IndexTrainer", "Admins");
                }
                AddErrors(result);
            }
            TempData["message"] = "Trainer Account has Successfully Create";
            return RedirectToAction("IndexTrainer", "Admins");
        }

        [HttpGet]
        public ActionResult EditTrainerAccount(string id)
        {
            var trainerInDb = _context.Trainers.SingleOrDefault(u => u.TrainerId == id);
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
            var trainerInfoInDb = _context.Trainers.SingleOrDefault(t => t.TrainerId == trainer.TrainerId);

            if (trainerInfoInDb == null)
            {
                return HttpNotFound();
            }
            trainerInfoInDb.FullName = trainer.FullName;
            trainerInfoInDb.Age = trainer.Age;
            trainerInfoInDb.Address = trainer.Address;
            trainerInfoInDb.Specialty = trainer.Specialty;

            _context.SaveChanges();

            TempData["message"] = "Trainer Info has Successfully Changed";
            return RedirectToAction("IndexTrainer", "Admins");
        }

        [HttpGet]
        public ActionResult DeleteTrainerAccount(string id)
        {
            var trainerInDb = _context.Users.SingleOrDefault(i => i.Id == id);
            var trainerInfoInDb = _context.Trainers.SingleOrDefault(i => i.TrainerId == id);
            if (trainerInDb == null || trainerInfoInDb == null)
            {
                return HttpNotFound();
            }
            _context.Users.Remove(trainerInDb);
            _context.Trainers.Remove(trainerInfoInDb);
            _context.SaveChanges();

            TempData["message"] = "Trainer Account has Successfully Deleted";
            return RedirectToAction("IndexTrainer", "Admins");
        }

        [HttpGet]
        public ActionResult TrainerInfoDetails(string id)
        {
            var trainerInfoInDb = _context.Trainers
                .SingleOrDefault(t => t.TrainerId == id);

            if (trainerInfoInDb == null)
            {
                return HttpNotFound();
            }
            return View(trainerInfoInDb);
        }

        [HttpGet]
        public ActionResult TrainerPasswordChange()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TrainerPasswordChange(ChangePasswordViewModels viewModel, string id)
        {
            var userInDb = _context.Users.SingleOrDefault(i => i.Id == id);
            if (userInDb == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            userId = userInDb.Id;

            if (userInDb == null)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                userManager.RemovePassword(userId);
                string newPassword = viewModel.NewPassword;
                userManager.AddPassword(userId, newPassword);
            }
            _context.SaveChanges();
            TempData["message"] = "Trainer password has Successfully changed";
            return RedirectToAction("IndexTrainer", "Admins");
        }
    }
}