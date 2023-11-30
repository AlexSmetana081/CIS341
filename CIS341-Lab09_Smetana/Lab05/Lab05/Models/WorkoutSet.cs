namespace Lab05.Models
{
    public class WorkoutSet
    {
        // Keys
        public int WorkoutSetId { get; set; }
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }

        public int Reps { get; set; }
        public int Sets { get; set; }
        public double Weight { get; set; }



        // Nav props
        public Workout Workout { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;
    }
}
