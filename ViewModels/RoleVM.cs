using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.ViewModels
{
    public class RoleVM
    {
        [Display(Name = "ID")]
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public int UsersCount { get; set; }

        public List<string> UserEmails { get; set; }
    }
}
