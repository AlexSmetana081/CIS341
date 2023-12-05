using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab05.Models
{
    public class TrackedWorkout
    {

        // Keys
        public int TrackedWorkoutId { get; set; }
        public int AccountId { get; set; }
        public int WorkoutId { get; set; }
        // Props
        [DataType(DataType.DateTime)]
        [Display(Name = "Completion Date")]
        public DateTime Completed { get; set; }
        // Nav props
        public Account Account { get; set; } = null!;
        public Workout Workout { get; set; } = null!;
    }
}
