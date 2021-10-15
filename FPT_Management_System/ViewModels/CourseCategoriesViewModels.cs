using FPT_Management_System.Models;
using System.Collections.Generic;

namespace FPT_Management_System.ViewModels
{
    public class CourseCategoriesViewModels
    {
        public Course Courses { get; set; }

        public List<CourseCategory> CourseCategories { get; set; }
    }
}