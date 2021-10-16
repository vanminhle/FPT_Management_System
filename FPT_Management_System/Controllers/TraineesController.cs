using FPT_Management_System.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPT_Management_System.Controllers
{
    [Authorize(Roles = Role.Trainee)]
    public class TraineesController : Controller
    {
        // GET: Trainees
        public ActionResult Index()
        {
            return View();
        }
    }
}