using Lab05.Models;
using System.ComponentModel.DataAnnotations;

namespace Lab05.ViewModels
{
    public class ExerciseDTO
    {
        // Gets or sets the content of the ExerciseID.
        public int ExerciseId { get; set; }

        // Gets or sets the content of the Name.
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        [Display(Name = "Exercise Name")]
        public string Name { get; set; } = string.Empty;

        // Gets or sets the content of the Description.
        [StringLength(500, ErrorMessage = "Description can be up to 500 characters.")]
        [Display(Name = "Exercise Description")]
        public string Description { get; set; } = string.Empty;

        // Gets or sets the content of the Author. 
        public int AuthorId { get; set; }
        public AccountModel Author { get; set; } = new AccountModel();

        // Gets or sets the content of the Length.
        public int Length { get; set; }

        // Gets or sets the content of the Intensity.
        [Range(1, 10, ErrorMessage = "Intensity must be between 1 and 10.")]
        [Display(Name = "Exercise Intensity")]
        public int Intensity { get; set; }
    }
}
