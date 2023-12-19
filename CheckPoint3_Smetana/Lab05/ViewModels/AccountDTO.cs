using Lab05.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab05.ViewModels
{
    public class AccountDTO
    {
        // Props
        [EmailAddress]
        [Required]
        public string Name { get; set; } = null!;

    }
}
