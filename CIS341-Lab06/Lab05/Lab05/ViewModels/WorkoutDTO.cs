using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Lab05.Models;

namespace Lab05.ViewModels
{
    public class WorkoutDTO
    {
        // Gets or sets the content of the Workout Id.
        public int WorkoutId { get; set; }

        // Gets or sets the content of the Workout Name.
        [Required(ErrorMessage = "Workout name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Workout name must be between 1 and 100 characters.")]
        [Display(Name = "Workout Name")]
        public string Name { get; set; } = string.Empty;

        // Gets or sets the content of the Author Id.
        [Required(ErrorMessage = "Author ID is required.")]
        [Display(Name = "Author ID")]
        public int AuthorId { get; set; }

        // Gets or sets the content of the Author.
        [Display(Name = "Author")]
        public AccountModel Author { get; set; } = new AccountModel();

        // Gets or sets the content of the Exercises.
        public List<ExerciseModel> Exercises { get; set; } = new List<ExerciseModel>();
    }
}
