using Lab05.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab05.ViewModels
{
    public class UserAccountDTO
    {
        public string Id { get; set; }
        public int AccountId { get; set; }

        // Props
        [EmailAddress]
        [Required]
        public string Name { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
