using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using site.Repositories;
using Site.Data;

namespace site.Pages.Admin.Stores
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly StoreRepository storeRepository;

        public IndexModel(ApplicationDbContext dbContext, StoreRepository _storeRepository)
        {
            _dbContext = dbContext;
            storeRepository = _storeRepository;
        }

        public List<StoreView> Stores { get; private set; }
        public const int PAGE_SIZE = 20;

        [FromQuery]
        public int PageIndex { get; set; }
        public int TotalRecords { get; private set; }
        public int TotalPages { get; private set; }

        public async Task OnGet()
        {
            PageIndex = PageIndex == 0 ? 1 : PageIndex;
            var result = await storeRepository.GetStoreList(PageIndex, PAGE_SIZE);
            Stores = result.Records;
            TotalRecords = result.TotalRecords;
            TotalPages = (int)Math.Ceiling(TotalRecords * 1.0 / PAGE_SIZE);
        }
    }
}
