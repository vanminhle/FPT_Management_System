using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPT_Management_System.Models
{
    public class TraineesToCourse
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Trainee")]
        public string TraineeId { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public Trainee Trainee { get; set; }

        public Course Course { get; set; }
    }
}