
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab05.Models
{
    public class Workout
    {
        // Keys
        public int WorkoutId { get; set; }
        public int AuthorId { get; set; }
        // Props
        [Required(ErrorMessage = "YYYYY")]
        [StringLength(50, ErrorMessage = "Workout name max length is {1} characters.")]
        public string Name { get; set; } = null!;
        [StringLength(1000, ErrorMessage = "Workout description max length {1} characters.")]
        public string Description { get; set; } = null!;

        // Nav props
        public Account Author { get; set; } = null!;
        public ICollection<WorkoutSet>? Exercises { get; set; } = null!;


    }
}
