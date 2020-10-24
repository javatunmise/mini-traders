using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Shared.Entities;
using site.Configs;
using site.Data;
using site.Data.Repositories;
using site.Helpers;
using site.Pages.Categories;
using site.Repositories;

namespace site.Pages.Profile.Store
{
    public class IndexModel : PageModel
    {
        private readonly AccountRepository _accountRepo;
        private readonly ProductsRepository _productRepository;
        private readonly StoreRepository _storeRepo;
        private readonly PaymentRepository _paymentRepo;
        private readonly PaymentConfig _paymentConfig;
        private readonly ISiteContentProvider _provider;

        public IndexModel(AccountRepository accountRepository,
                ProductsRepository productRepository,
                PaymentRepository paymentRepository,
                StoreRepository storeRepository,
                IOptions<PaymentConfig> paymentConfig,
                ISiteContentProvider provider)
        {
            _accountRepo = accountRepository;
            SearchQuery = new ProductSearchQuery();
            _productRepository = productRepository;
            _storeRepo = storeRepository;
            _paymentRepo = paymentRepository;
            _paymentConfig = paymentConfig.Value;
            _provider = provider;
        }

        public async Task<IActionResult> OnGet(ProductSearchQuery query)
        {
            var email = User.Identity.Name;
            var currentUser = await _accountRepo.FindByUsername(email);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var store = currentUser.Store;
            var storeDetails = await _storeRepo.GetStoreById(store.Id);

            if (storeDetails.Status == StoreStatuses.Suspended ||
                storeDetails.Status == StoreStatuses.Deactivated)
            {
                return Redirect($"/Profile/Store/InvalidStatus/{storeDetails.Status}");
            }

            StoreHasNotBeenActivated = storeDetails.Status == StoreStatuses.Inactive;
            if (StoreHasNotBeenActivated)
            {
                var site = await _provider.GetCurrentSite();
                ActivationPaymentCode = GeneratePaymentRef(storeDetails);
                ActivationAmount = site.SignOnFee;
                await _paymentRepo.CreatePayment(new ActivateStorePaymentReservationData(store, ActivationAmount, ActivationPaymentCode, email)
                {
                    Charge = _paymentConfig.SignUpCharge
                });
            }

            IEnumerable<Product> products = query.FilterByServices ? await _productRepository.GetServices(store.Id) :
                                                                     await _productRepository.GetProducts(store.Id);

            Products = products;
            SearchQuery = query;
            return Page();
        }

        private string GeneratePaymentRef(Shared.Entities.Store storeDetails)
        {
            var initials = storeDetails.StoreName?.Trim().DefaultTo("A")[0];
            return $"{storeDetails.Id}{initials}{DateTime.Now:yyyyMMddss}";
        }

        public IEnumerable<Product> Products { get; private set; }

        public ProductSearchQuery SearchQuery { get; set; }
        public bool StoreHasNotBeenActivated { get; private set; }
        public string ActivationPaymentCode { get; private set; }
        public decimal ActivationAmount { get; private set; }
    }

    public class ProductSearchQuery
    {
        public string Filter { get; set; }
        public bool FilterByServices => Filter?.ToLower() == "services";
    }
}