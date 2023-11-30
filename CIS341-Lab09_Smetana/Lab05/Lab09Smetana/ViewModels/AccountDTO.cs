using Lab09Smetana.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab09Smetana.ViewModels
{
    public class AccountDTO
    {
        // Props
        [EmailAddress]
        [Required]
        public string Name { get; set; } = null!;

    }
}
