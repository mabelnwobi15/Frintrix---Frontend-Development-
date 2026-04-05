using Microsoft.AspNetCore.Mvc;
using SmartInvoice.MVC.Models;
using SmartInvoice.MVC.Services;

namespace SmartInvoice.MVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Display all payments
        public async Task<IActionResult> Index()
        {
            var paymentsList = await _paymentService.GetPaymentsByUser(); // returns List<PaymentListViewModel>

            // Map to PaymentViewModel
            var payments = paymentsList.Select(p => new PaymentViewModel
            {
                Id = p.Id,
                InvoiceId = p.InvoiceId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
            }).ToList();

            return View(payments); // now this matches @model List<PaymentViewModel>
        }

        // Create a new payment
        [HttpPost]
        public async Task<IActionResult> Create(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                TempData["Error"] = "Invalid form data: " + errors;
                return RedirectToAction("Index");
            }

            var result = await _paymentService.CreatePayment(model);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Payment recorded successfully!";
            return RedirectToAction("Index");
        }
    }
}