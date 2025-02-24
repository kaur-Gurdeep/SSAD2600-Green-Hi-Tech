using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenHiTech.ViewModels
{
    public class PaymentVM
    {
        [Display(Name = "Payment ID")]
        public int PkId { get; set; }

        [Required]
        [Display(Name = "Order ID")]
        public int FkOderId { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        public DateOnly PaymentDate { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(9,2)")]
        [Range(0.01, 999999.99, ErrorMessage = "Amount must be between at least $0.01 and @999999.99")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Transaction ID")]
        public int TransactionId { get; set; }

        // Additional display properties
        [Display(Name = "Order Total")]
        public decimal? OrderTotal { get; set; }

        [Display(Name = "Payment Status")]
        public string? PaymentStatus { get; set; }

        [Display(Name = "Customer Name")]
        public string? CustomerName { get; set; }
    }
}