using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.ViewModels
{
    public class UserRoleVM
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Email")]
        [Required]
        public string Email { get; set; } 

        [Display(Name = "Role")]
        [Required]
        public string Role { get; set; } 
    }
}
