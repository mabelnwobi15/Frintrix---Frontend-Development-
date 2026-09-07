namespace SmartInvoice.MVC.Models
{
    public class InvoiceViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ClientName { get; set; }
    }

    public class InvoiceItemViewModel
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateInvoiceViewModel
    {
        public int ClientId { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public List<InvoiceItemViewModel> Items { get; set; } = new();
    }
}