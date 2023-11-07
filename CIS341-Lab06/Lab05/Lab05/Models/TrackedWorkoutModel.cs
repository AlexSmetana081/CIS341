using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab05.Models
{
    public class TrackedWorkoutModel
    {
        // Gets or sets the content of the TrackedWorkoutID.
        public int TrackedWorkoutID { get; set; }

        // Gets or sets the content of the WorkoutID Id.
        [Required(ErrorMessage = "Workout ID is required.")]
        public int WorkoutID { get; set; }

        // Gets or sets the content of the Workout.
        public WorkoutModel Workout { get; set; } = new WorkoutModel();

        // Gets or sets the content of the Date Completed.
        [Required(ErrorMessage = "Date completed is required.")]
        [Display(Name = "Date Completed")]
        [DataType(DataType.Date)]
        public DateTime DateCompleted { get; set; }

        // Gets or sets the content of the Account Id.
        [Required(ErrorMessage = "Account ID is required.")]
        public int AccountID { get; set; }

        // Gets or sets the content of the Account.
        public AccountModel Account { get; set; } = new AccountModel();
    }
}
