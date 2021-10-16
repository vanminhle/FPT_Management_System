using FPT_Management_System.Models;

namespace FPT_Management_System.ViewModels
{
    public class TrainerAccountViewModels
    {
        public RegisterViewModel RegisterViewModels { get; set; }

        public Trainer Trainers { get; set; }

        public ApplicationUser TrainerUsers { get; set; }
    }
}