namespace Lab09Smetana.Models
{
    public class WorkoutSet
    {
        // Keys
        public int WorkoutSetId { get; set; }
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }
        // Nav props
        public Workout Workout { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;
    }
}
