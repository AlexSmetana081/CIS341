using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Lab05.Models
{
    public class RoleModel
    {
        // Gets or sets the content of the Role Id.
        public int RoleId { get; set; }

        // Gets or sets the content of the Name.
        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Role name must be between 1 and 100 characters.")]
        [Display(Name = "Role Name")]
        public string Name { get; set; } = string.Empty;

    }
}
