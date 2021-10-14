using FPT_Management_System.Models;
using System.Collections.Generic;

namespace FPT_Management_System.ViewModels
{
    public class StaffAccountViewModels
    {
        public RegisterViewModel RegisterViewModels { get; set; }

        public Staff Staffs { get; set; }

        public List<Staff> GetStaffsInfo { get; set; }

        public ApplicationUser StaffUsers { get; set; }

        public List<ApplicationUser> GetStaffsUsers { get; set; }
    }
}