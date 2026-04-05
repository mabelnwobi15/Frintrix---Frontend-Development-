using System;
using System.ComponentModel.DataAnnotations;

namespace SmartInvoice.MVC.Models
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Invoice ID is required")]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}