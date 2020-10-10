using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Payment.Paystack;
using site.Configs;
using site.Data.Repositories;
using site.Helpers;
using static Shared.PaymentStatuses;

namespace site.Pages.Payments
{
    public class IndexModel : PageModel
    {
        private readonly PaymentConfig _paymentConfig;
        private readonly PaymentRepository _paymentRepo;
        private readonly HttpClient client;

        public IndexModel(IOptions<PaymentConfig> paymentConfig,
            PaymentRepository paymentRepository,
            IHttpClientFactory httpClientFactory)
        {
            _paymentConfig = paymentConfig.Value;
            _paymentRepo = paymentRepository;
            client = httpClientFactory.CreateClient("paystack");
        }

        public string ErrorMessage { get; private set; }

        public async Task<IActionResult> OnGet(string pRef)
        {
            var payment = await _paymentRepo.GetPayment(pRef);
            if (payment == null) return RedirectToPage("/Error404");
            if (payment.Status != Initiated) return RedirectToPage("/Error404");

            var jsonRequest = JsonConvert.SerializeObject(new
            {
                email = payment.Email,
                amount = payment.Amount * 100,
                reference = payment.PaymentRef
            });

            var responseMessage = await client.PostAsync("transaction/initialize", new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json"));

            var response = JsonConvert.DeserializeObject<InitiatePaymentResponse>(await responseMessage.Content.ReadAsStringAsync());
            
            if(response.status) return Redirect(response.data.authorization_url);

            ErrorMessage = $"{responseMessage.StatusCode}: {response.message}";

            return Page();
        }
    }
}

namespace Payment.Paystack
{
    public class Data
    {
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public string reference { get; set; }
    }

    public class InitiatePaymentResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }


}