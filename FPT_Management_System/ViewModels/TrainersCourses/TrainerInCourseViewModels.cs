using FPT_Management_System.Models;
using System.Collections.Generic;

namespace FPT_Management_System.ViewModels
{
    public class TrainerInCourseViewModels
    {
        //index
        public Course Course { get; set; }

        public List<Trainer> Trainers { get; set; }
    }
}