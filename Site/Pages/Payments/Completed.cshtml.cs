using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using site.Data.Repositories;
using site.Helpers.Services;
using site.Repositories;

namespace site.Pages.Payments
{
    public class CompletedModel : PageModel
    {
        private readonly PaymentRepository paymentRepository;
        private readonly StoreRepository storeRepository;
        private readonly StoreActivationHandler storeActivationHandler;

        //trxref=te4lsik8b5&reference=te4lsik8b5

        [FromQuery(Name = "trxref")]
        public string TrxRef { get; set; }

        [FromQuery(Name = "reference")]
        public string PaymentRef { get; set; }
        public bool ErrorOccurred { get; private set; }
        public string ErrorMessage { get; private set; }

        public CompletedModel(PaymentRepository paymentRepository, 
                              StoreRepository storeRepository,
                              StoreActivationHandler activateStoreHandler)
        {
            this.paymentRepository = paymentRepository;
            this.storeRepository = storeRepository;
            this.storeActivationHandler = activateStoreHandler;
        }
        public async Task<IActionResult> OnGet()
        {
            var payment = await paymentRepository.GetPayment(PaymentRef);
            if (payment == null) return NotFound();

            payment.PaymentDate = DateTime.Now;
            payment.Status = Shared.PaymentStatuses.Completed;
            payment.ExternalRef = TrxRef;

            await paymentRepository.Update(payment);

            var store = await storeRepository.GetStoreById(int.Parse(payment.EntityId));
            var referrer = await storeRepository.GetRefererStoreById(store.Id);

            var result = storeActivationHandler.Handle(payment, store, referrer);
            if (result == null) return InvalidOperationPage("Invalid Payment");

            await storeRepository.SaveActivationResult(result);            

            return Page();
        }

        private IActionResult InvalidOperationPage(string message)
        {
            ErrorOccurred = true;
            ErrorMessage = message;
            return Page();
        }
    }
}
