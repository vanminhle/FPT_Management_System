﻿using FPT_Management_System.Models;
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

        public AdminsController()
        {
            _context = new ApplicationDbContext();
        }

        public AdminsController(ApplicationUserManager userManager)
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

        [HttpGet]
        public ActionResult Index()
        {
            /*var role = _context.Roles.SingleOrDefault(n => n.Name == Role.Staff);

            List<StaffAccountViewModels> viewModel = _context.Staffs
                .GroupBy(u => u.User, s => s.StaffId)
                .Select(res => new StaffAccountViewModels
                {
                    StaffUsers = res.Key,
                    GetStaffsUsers = _context.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).ToList(),
                    Staffs = res.Key
                }).ToList();*/
            var staffsList = _context.Staffs.ToList();
            return View(staffsList);
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
                    Address = viewModel.Staffs.Address
                };

                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, Role.Staff);
                    _context.Staffs.Add(newStaff);
                    _context.SaveChanges();
                }
                AddErrors(result);
            }

            return RedirectToAction("Index", "Admins");
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

            return RedirectToAction("Index", "Admins");
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
            return RedirectToAction("Index", "Admins");
        }

        [HttpGet]
        public ActionResult StaffInfoDetails(string id)
        {
            var staffId = User.Identity.GetUserId();

            var staffInfoInDb = _context.Staffs
                .SingleOrDefault(t => t.StaffId == id);

            if (staffInfoInDb == null)
            {
                return HttpNotFound();
            }
            return View(staffInfoInDb);
        }

        public ActionResult StaffPasswordReset(string id)
        {
            var staffInDb = _context.Users.SingleOrDefault(i => i.Id == id);
            if (staffInDb == null)
            {
                return HttpNotFound();
            }
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            userId = staffInDb.Id;
            if (userId != null)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                userManager.RemovePassword(userId);
                string newPassword = "DefaultPassword@123";
                userManager.AddPassword(userId, newPassword);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Admins");
        }
    }
}