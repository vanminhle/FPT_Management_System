using FPT_Management_System.Models;
using System.Collections.Generic;

namespace FPT_Management_System.ViewModels
{
    public class TrainerAccountViewModels
    {
        public RegisterViewModel RegisterViewModels { get; set; }

        public Trainer Trainers { get; set; }

        public List<Trainer> GetTrainerInfo { get; set; }

        public ApplicationUser TrainerUsers { get; set; }

        public List<ApplicationUser> GetTrainerUsers { get; set; }
    }
}