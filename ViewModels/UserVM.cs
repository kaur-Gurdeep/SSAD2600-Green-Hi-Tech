using System.ComponentModel.DataAnnotations;

namespace GreenHiTech.ViewModels
{
    public class UserVM
    {
        public int PkUserId { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "User Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        public string? Role { get; set; }

        public string Phone { get; set; } = null!;
        public AddressDetailVM? AddressDetail { get; set; }
    }

}
