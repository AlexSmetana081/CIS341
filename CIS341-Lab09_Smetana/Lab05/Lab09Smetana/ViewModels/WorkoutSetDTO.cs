using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lab09Smetana.Models;

namespace Lab09Smetana.ViewModels
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
