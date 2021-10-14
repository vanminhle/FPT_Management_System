using System.ComponentModel.DataAnnotations;

namespace FPT_Management_System.Models
{
    public class CourseCategory
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter Category Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Category Description")]
        [StringLength(255)]
        public string Description { get; set; }
    }
}