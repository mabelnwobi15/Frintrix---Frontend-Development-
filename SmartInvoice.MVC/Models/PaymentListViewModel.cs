using System.ComponentModel.DataAnnotations;

namespace SmartInvoice.MVC.Models
{
    public class PaymentListViewModel
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string ClientName { get; set; }       // Customer name
        public string ClientEmail { get; set; }      // Customer email
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}