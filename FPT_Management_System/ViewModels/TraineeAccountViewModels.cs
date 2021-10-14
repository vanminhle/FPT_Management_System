using FPT_Management_System.Models;
using System.Collections.Generic;

namespace FPT_Management_System.ViewModels
{
    public class TraineeAccountViewModels
    {
        public RegisterViewModel RegisterViewModels { get; set; }

        public Trainee Trainees { get; set; }

        public List<Trainee> GetTraineesInfo { get; set; }

        public ApplicationUser TraineeUsers { get; set; }

        public List<ApplicationUser> GetTraineeUsers { get; set; }
    }
}