using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GreenHiTech.Models;

namespace GreenHiTech.ViewModels
{
    public class AddressDetailVM
    {
        public int PkId { get; set; }

        [StringLength(50, ErrorMessage = "Unit cannot be longer than 50 characters.")]
        public string? Unit { get; set; }

        [StringLength(20, ErrorMessage = "House number cannot be longer than 20 characters.")]
        public string? HouseNumber { get; set; }

        [StringLength(100, ErrorMessage = "Street cannot be longer than 100 characters.")]
        public string? Street { get; set; }

        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "Province cannot be longer than 50 characters.")]
        public string? Province { get; set; }

        [StringLength(20, ErrorMessage = "Postal code cannot be longer than 20 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s\-]+$", ErrorMessage = "Invalid postal code format.")]
        public string? PostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
        public string? Country { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
