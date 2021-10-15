using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPT_Management_System.Models
{
    public class TrainersToCourse
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Trainer")]
        public string TrainerId { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public Trainer Trainer { get; set; }

        public Course Course { get; set; }
    }
}