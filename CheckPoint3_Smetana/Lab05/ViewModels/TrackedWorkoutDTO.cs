using System;
using System.ComponentModel.DataAnnotations;
using Lab05.Models;

namespace Lab05.ViewModels
{
    public class TrackedWorkoutDTO
    {
        public int TrackedWorkoutId { get; set; }
        public int AccountId { get; set; }
        public int WorkoutId { get; set; }
        // Props
        [DataType(DataType.DateTime)]
        [Display(Name = "Completion Date")]
        public DateTime Completed { get; set; }
    }
}
