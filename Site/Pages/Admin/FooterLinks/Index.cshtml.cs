using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Site.Data;

namespace site.Pages.Admin.FooterLinks
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDictionary<int, SiteLink.LinkGroups> _enums;

        public IndexModel(ApplicationDbContext context)
        {
            _dbContext = context;
            _enums = new Dictionary<int, SiteLink.LinkGroups>
            {
                { (int)SiteLink.LinkGroups.FooterLinkHelpAndSupport, SiteLink.LinkGroups.FooterLinkHelpAndSupport },
                { (int)SiteLink.LinkGroups.FooterLinkCustomerService, SiteLink.LinkGroups.FooterLinkCustomerService },
                { (int)SiteLink.LinkGroups.FooterLinkCorporation, SiteLink.LinkGroups.FooterLinkCorporation },
                { (int)SiteLink.LinkGroups.FooterLinkWhyChoseUs, SiteLink.LinkGroups.FooterLinkWhyChoseUs },
            };
        }

        public List<SiteLink> SiteLinks { get; private set; }

        [BindProperty]
        public FormInput Input { get; set; }

        public async Task<IActionResult> OnGet(int footerheader = 1)
        {
            if (!_enums.TryGetValue(footerheader, out SiteLink.LinkGroups valid))
                return RedirectToPage("/Error404");

            SiteLinks = await _dbContext.SiteLinks.Where(e => e.LinkGroup == valid).ToListAsync();

            Input = new FormInput { GroupId = footerheader };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!_enums.TryGetValue(Input.GroupId, out SiteLink.LinkGroups valid))
                return RedirectToPage("/Error404");

            _dbContext.SiteLinks.Add(new SiteLink
            {
                LinkGroup = valid, Text = Input.Text, 
                Url = Input.Link
            });

            await _dbContext.SaveChangesAsync();

            return Redirect($"/Admin/FooterLinks?footerheader={Input.GroupId}");
        }

        public class FormInput
        {
            public string Link { get; set; }

            [Required]
            public string Text { get; set; }
            public int GroupId { get; set; }
        }
    }
}