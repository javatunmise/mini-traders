using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using site.Data;
using site.Helpers;
using Site.Data;

namespace site.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ISiteContentProvider _provider;
        private readonly IConfiguration configuration;

        public IndexModel(ApplicationDbContext context, ISiteContentProvider siteContentProvider, IConfiguration configuration)
        {
            _dbContext = context;
            _provider = siteContentProvider;
            this.configuration = configuration;
        }

        public async Task OnGet()
        {
            if (!StoreUtil.IsAdmin(User.Identity.Name, configuration))
            {
                Response.Redirect("/");
                return;
            }

            var site = await _provider.GetCurrentSite() ?? new Shared.Entities.Site();

            Input = new FormInput
            {
                AboutUsIntro = site.AboutUsIntro,
                CopyRightName = site.CopyRightName,
                Address = site.Address,
                Email = site.Email,
                PhoneNumber = site.PhoneNumber,
                FacebookUrl = site.FacebookUrl,
                InstagramUrl = site.InstagramUrl,
                LinkedInUrl = site.LinkedInUrl,
                YoutubeUrl = site.YoutubeUrl,
                TwitterUrl = site.TwitterUrl,
                GoogleAnalyticsScript = site.GoogleAnalyticsScript,
                IsGoogleAnalyticsEnabled = site.IsGoogleAnalyticsEnabled,
                WorkingHours = site.WorkingHours,                
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var siteId = new Guid(Shared.Entities.Site.Identifier);
            var site = await _dbContext.Sites.FirstOrDefaultAsync(e => e.Id == siteId);
            var isNew = site == null;
            site = site ?? new Shared.Entities.Site { Id = siteId };

            site.AboutUsIntro = Input.AboutUsIntro;
            site.CopyRightName = Input.CopyRightName;
            site.Address = Input.Address;
            site.Email = Input.Email;
            site.PhoneNumber = Input.PhoneNumber;
            site.FacebookUrl = Input.FacebookUrl;
            site.InstagramUrl = Input.InstagramUrl;
            site.LinkedInUrl = Input.LinkedInUrl;
            site.YoutubeUrl = Input.YoutubeUrl;
            site.TwitterUrl = Input.TwitterUrl;
            site.GoogleAnalyticsScript = Input.GoogleAnalyticsScript;
            site.IsGoogleAnalyticsEnabled = Input.IsGoogleAnalyticsEnabled;
            site.WorkingHours = Input.WorkingHours;

            if (isNew)
                _dbContext.Sites.Add(site);

            await _dbContext.SaveChangesAsync();

            return Page();
        }

        [BindProperty]
        public FormInput Input { get; set; }

        public class FormInput
        {
            public string AboutUsIntro { get; set; }
            public string CopyRightName { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string WorkingHours { get; set; }
            public string FacebookUrl { get; set; }
            public string YoutubeUrl { get; set; }
            public string InstagramUrl { get; set; }
            public string LinkedInUrl { get; set; }
            public string TwitterUrl { get; set; }

            public bool IsGoogleAnalyticsEnabled { get; set; }
            public string GoogleAnalyticsScript { get; set; }
        }
    }
}