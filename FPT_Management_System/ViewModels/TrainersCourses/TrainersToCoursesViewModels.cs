using FPT_Management_System.Models;
using System.Collections.Generic;

namespace FPT_Management_System.ViewModels
{
    //assign, remove
    public class TrainersToCoursesViewModels
    {
        public int CourseId { get; set; }
        public string TrainerId { get; set; }

        public List<Course> Courses { get; set; }
        public List<Trainer> Trainers { get; set; }
    }
}