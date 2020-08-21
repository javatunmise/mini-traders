using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace site.Pages.Profile.Store
{
    public class SettingsModel : PageModel
    {
        public void OnGet()
        {

        }

        public StoreEdit Input { get; set; }
    }

    public class StoreEdit
    {
        [Required]
        [Display(Name = "Business Name")]
        public string StoreName { get; set; }

        public string StoreDescription { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone Number")]
        public string ContactPhone { get; set; }

    }
}