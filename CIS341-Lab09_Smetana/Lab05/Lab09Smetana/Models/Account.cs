using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace Lab09Smetana.Models
{
    public class Account
    {
        // Keys
        public int AccountId { get; set; }
        // Props
        [EmailAddress]
        //[Required]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;
        // Nav props

        public ICollection<TrackedWorkout>? Workouts { get; set; } = null!;
        public ICollection<Message>? Messages { get; set; } = null!;
    }
}
