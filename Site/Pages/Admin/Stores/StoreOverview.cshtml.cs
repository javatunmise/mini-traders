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
    public class StoreOverviewModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly StoreRepository storeRepository;

        public StoreOverviewModel(ApplicationDbContext context, StoreRepository storeRepository)
        {
            this.context = context;
            this.storeRepository = storeRepository;
        }

        public Store CurrentStore { get; private set; }

        public async Task<IActionResult> OnGet(int id)
        {
            CurrentStore = await context.Stores
                                 .Include(x => x.Campus)
                                 .Include(x => x.Hostel)
                                 .Include(x => x.User)
                                 .FirstOrDefaultAsync(x => x.Id == id);

            return Page();
        }
    }
}
