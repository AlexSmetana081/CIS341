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

    }
}
