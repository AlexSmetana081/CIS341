using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lab05.Models;

namespace Lab05.ViewModels
{
    public class WorkoutSetDTO
    {
        public int WorkoutSetId { get; set; }

        [Required]
        public int WorkoutId { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reps must be a non-negative value.")]
        public int Reps { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Reps must be a non-negative value.")]
        public int Sets { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Weight must be a non-negative value.")]
        public double Weight { get; set; }

    }
}
