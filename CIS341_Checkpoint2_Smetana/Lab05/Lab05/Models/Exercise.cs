using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab05.Models
{
    public class Exercise
    {
        // Keys
        public int ExerciseId { get; set; }
        public int AccountId { get; set; }
        // Props
        [Required(ErrorMessage = "XXXXX")]
        [StringLength(50, ErrorMessage = "Exercise name max length is {1} characters.")]
        public string Name { get; set; } = null!;
        [StringLength(1000, ErrorMessage = "Exercise description max length is {1} characters.")]
        public string Description { get; set; } = null!;
        [Display(Name = "Intensity")]
        public Intensity WorkoutIntensity { get; set; }
        // Nav props
        public Account AuthorName { get; set; } = null!;

    }
}
