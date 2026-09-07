using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using SmartInvoice.MVC.Models;

namespace SmartInvoice.MVC.Services
{
    public class InvoiceService
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InvoiceService(
            IHttpClientFactory httpFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _http = httpFactory.CreateClient("API");
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?
                .Session.GetString("JWToken");

            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<InvoiceViewModel>> GetInvoices()
        {
            AddAuthHeader();

            var response = await _http.GetAsync("api/invoices");

            if (!response.IsSuccessStatusCode)
            {
                return new List<InvoiceViewModel>();
            }

            var invoices = await response.Content
                .ReadFromJsonAsync<List<InvoiceViewModel>>();

            return invoices ?? new List<InvoiceViewModel>();
        }

        public async Task<bool> CreateInvoice(CreateInvoiceViewModel invoice)
        {
            AddAuthHeader();

            var request = new
            {
                ClientId = invoice.ClientId,
                DueDate = invoice.DueDate,
                Items = invoice.Items.Select(i => new
                {
                    i.Description,
                    i.Quantity,
                    i.Price
                }).ToList()
            };

            var response = await _http.PostAsJsonAsync(
                "api/invoices",
                request
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<InvoiceViewModel?> GetInvoiceById(int id)
        {
            AddAuthHeader();

            var invoices = await _http.GetFromJsonAsync<List<InvoiceViewModel>>(
                "api/invoices"
            );

            return invoices?.FirstOrDefault(i => i.Id == id);
        }
    }
}