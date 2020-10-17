using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Site.Data;

namespace site.Pages.Admin.Stores
{
    public class ActivateModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ActivateModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FromRoute]
        public int Id { get; set; }
        [FromQuery]
        public int PageIndex { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var store = await _dbContext.Stores.FirstOrDefaultAsync(e => e.Id == Id);
            if (store == null)
                return Redirect("/Admin/Stores");

            if (store.Status != Shared.Entities.StoreStatuses.Active)
            {
                store.Status = Shared.Entities.StoreStatuses.Active;
                await _dbContext.SaveChangesAsync();
            }

            return Redirect($"/Admin/Stores?pageindex={PageIndex}");
        }
    }
}
