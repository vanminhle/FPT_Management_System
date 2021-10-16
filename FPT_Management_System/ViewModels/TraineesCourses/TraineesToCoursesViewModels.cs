using FPT_Management_System.Models;
using System.Collections.Generic;

namespace FPT_Management_System.ViewModels
{
    public class TraineesToCoursesViewModels
    {
        public int CourseId { get; set; }
        public string TraineeId { get; set; }
        public List<Course> Courses { get; set; }

        public List<Trainee> Trainees { get; set; }
    }
}