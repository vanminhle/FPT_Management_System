using FPT_Management_System.Models;

namespace FPT_Management_System.ViewModels
{
    public class TraineeAccountViewModels
    {
        public RegisterViewModel RegisterViewModels { get; set; }

        public Trainee Trainees { get; set; }

        public ApplicationUser TraineeUsers { get; set; }
    }
}