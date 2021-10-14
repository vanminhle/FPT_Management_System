using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPT_Management_System.Models
{
    public class Trainer
    {
        [Key]
        [ForeignKey("User")]
        //khoa ngoai vs bang user
        public string TrainerId { get; set; }

        public ApplicationUser User { get; set; }

        //for validation
        [Required(ErrorMessage = "Please enter Trainer Name")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Trainer Name Should Between 4 to 30 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter Trainer Age")]
        [Range(20, 80, ErrorMessage = "Training Staff Age Should Between 20 to 80")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please enter Trainer Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter Trainer Specialty")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Trainer Specialty Should At least 4 to 30 characters")]
        public string Specialty { get; set; }
    }
}