using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace Lab05.Models
{
    public class AccountModel
    {
        // Gets or sets the content of the Account Id.
        public int AccountId { get; set; }

        // Gets or sets the content of the Name.
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        [Display(Name = "Account Name")]
        public string Name { get; set; } = string.Empty;

        // Gets or sets the content of the Email.
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Gets or sets the content of the Password.
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        //Link from Role to Account
        public int RoleId { get; set; }

        // Gets or sets the content of the Account Role.
        [Display(Name = "Account Role")]
        public RoleModel Role { get; set; } = new RoleModel();

        // Gets or sets the content of the Tracked Workouts.
        public List<TrackedWorkoutModel> TrackedWorkouts { get; set; } = new List<TrackedWorkoutModel>();

    }
}
