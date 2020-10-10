using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using site.Data;
using site.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class ProductsGrid_Recommended : ViewComponent
    {
        private readonly ISiteContentProvider _provider;
        private readonly AccountRepository _accountRepo;

        public ProductsGrid_Recommended(ISiteContentProvider provider, AccountRepository accountRepository)
        {
            _provider = provider;
            _accountRepo = accountRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = await _accountRepo.FindSiteUser(User.Identity.Name);
            List<Product> products = (await _provider.GetRecommended(currentUser)).ToList();
            return View(products);
        }
    }
}