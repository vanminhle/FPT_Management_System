using FPT_Management_System.Models;
using System.Collections.Generic;

namespace FPT_Management_System.ViewModels
{
    public class TraineeInCourseViewModels
    {
        //index
        public Course Course { get; set; }

        public List<Trainee> Trainees { get; set; }

        public ApplicationUser ApplicationUsers { get; set; }
    }

}