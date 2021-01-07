using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using site.Data;
using site.Repositories;
using Site.Data;

namespace site.Pages.Profile.Store
{
    public class FlashDealsModel : PageModel
    {
        private readonly ISiteContentProvider _provider;
        private readonly AccountRepository _accountRepo;
        private readonly ProductsRepository _productRepository;
        private readonly StoreRepository _storeRepo;
        private readonly ApplicationDbContext _dbContext;

        public FlashDealsModel(AccountRepository accountRepository,
                ProductsRepository productRepository,
                StoreRepository storeRepository, 
                ISiteContentProvider siteContentProvider,
                ApplicationDbContext dbContext)
        {
            _provider = siteContentProvider;
            _accountRepo = accountRepository;
            _productRepository = productRepository;
            _storeRepo = storeRepository;
            _dbContext = dbContext;
        }

        public IEnumerable<Product> Products { get; private set; }
        public List<FlashDealProduct> FlashDealProducts { get; private set; }
        public FlashDeal FlashDeal { get; set; }

        [Required]
        [BindProperty]
        public int ProductId { get; set; }
        [BindProperty]
        public decimal Discount { get; set; }
        [BindProperty]
        public decimal ProductPrice { get; set; }

        public async Task<IActionResult> OnGet(string a = "", int productId = 0)
        {
            var email = User.Identity.Name;
            var currentUser = await _accountRepo.FindByUsername(email);
            if (!currentUser.HasStore)
                return RedirectToPage("/Profile/Index");

            var store = currentUser.Store;
            var storeDetails = await _storeRepo.GetStoreById(store.Id);

            var ongoingFlashDeal = await _provider.GetCurrentFlashDeal();
            if (ongoingFlashDeal == null) return Page();

            Products = await _productRepository.GetAllProducts(store.Id);

            FlashDealProducts = await _dbContext.FlashDealProducts.AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.FlashDealId == ongoingFlashDeal.Id && x.StoreId == store.Id).ToListAsync();

            FlashDeal = ongoingFlashDeal;

            if(a == "rem")
            {
                _dbContext.Entry(new FlashDealProduct { ProductId = productId, FlashDealId = ongoingFlashDeal.Id }).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();
                return RedirectToPage();
            }

            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            var email = User.Identity.Name;
            var currentUser = await _accountRepo.FindByUsername(email);
            var store = currentUser.Store;
            var storeDetails = await _storeRepo.GetStoreById(store.Id);

            var ongoingFlashDeal = await _provider.GetCurrentFlashDeal();
            FlashDeal = ongoingFlashDeal;

            FlashDealProducts = await _dbContext.FlashDealProducts.AsNoTracking().Where(x => x.FlashDealId == ongoingFlashDeal.Id && x.StoreId == store.Id).ToListAsync();
            if (Discount / 100 < ongoingFlashDeal.MinimumDiscount)
            {
                await Populate(store.Id);
                ModelState.AddModelError("Discount", $"Discount amount must be greater than or equal {(int) (ongoingFlashDeal.MinimumDiscount * 100)}");
                return Page();
            }

            if (!FlashDealProducts.Select(x => x.ProductId).Contains(ProductId))
            {
                _dbContext.FlashDealProducts.Add(new FlashDealProduct
                {
                    CurrentPrice = ProductPrice - (ProductPrice * Discount / 100),
                    Discount = Discount / 100,
                    FlashDealId = ongoingFlashDeal.Id,
                    OldPrice = ProductPrice,
                    StoreId = store.Id,
                    ProductId = ProductId
                });

                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        private async Task Populate(int id)
        {
            Products = await _productRepository.GetAllProducts(id);
        }
    }
}
