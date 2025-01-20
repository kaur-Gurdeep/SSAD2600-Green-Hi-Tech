using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.ViewModels
{
    public class IdentityUserVM
    {
        [Display(Name = "Email")]
        public string? Email { get; set; }

    }
}
