using System;
using System.ComponentModel.DataAnnotations;
using Lab09Smetana.Models;

namespace Lab09Smetana.ViewModels
{
    public class TrackedWorkoutDTO
    {
        // Gets or sets the content of the TrackedWorkoutID.
        public int TrackedWorkoutID { get; set; }

        // Gets or sets the content of the WorkoutID Id.
        [Required(ErrorMessage = "Workout ID is required.")]
        public int WorkoutID { get; set; }

        // Gets or sets the content of the Workout.
        public Workout Workout { get; set; } = new Workout();

        // Gets or sets the content of the Date Completed.
        [Required(ErrorMessage = "Date completed is required.")]
        [Display(Name = "Date Completed")]
        [DataType(DataType.Date)]
        public DateTime DateCompleted { get; set; }

        // Gets or sets the content of the Account Id.
        [Required(ErrorMessage = "Account ID is required.")]
        public int AccountID { get; set; }

        // Gets or sets the content of the Account.
        public Account Account { get; set; } = new Account();
    }
}
