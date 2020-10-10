using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Entities;

namespace site.Pages.Profile.Store
{
    public class InvalidStatusModel : PageModel
    {
        public StoreStatuses StoreStatus { get; private set; }

        public void OnGet(StoreStatuses status)
        {
            this.StoreStatus = status;
        }
    }
}
