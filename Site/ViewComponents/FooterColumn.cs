using Microsoft.AspNetCore.Mvc;
using site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class FooterColumn : ViewComponent
    {
        public const string HELP_AND_SUPPORT = "HELP_AND_SUPPORT";
        public const string CUSTOMER_SERVICE = "CUSTOMER_SERVICE";
        public const string CORPORATION = "CORPORATION";
        public const string WHY_CHOOSE_US = "WHY_CHOOSE_US";

        private readonly ISiteContentProvider _provider;

        public FooterColumn(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync(string section)
        {
            await Task.CompletedTask;
            var linkSection = new FooterLinks("", new List<FooterLink>());

            if (section == HELP_AND_SUPPORT)
            {
                var footerColumn = await _provider.GetFooterHelpAndSupportLinks();

                linkSection = footerColumn ?? linkSection;
            }
            else if (section == CUSTOMER_SERVICE)
            {
                var footerColumn = await _provider.GetFooterCustomerServiceLinks();

                linkSection = footerColumn ?? linkSection;
            }
            else if (section == CORPORATION)
            {
                var footerColumn = await _provider.GetFooterCorporationSectionLinks();

                linkSection = footerColumn ?? linkSection;
            }
            else if (section == WHY_CHOOSE_US)
            {
                var footerColumn = await _provider.GetFooterWhyUsLinks();

                linkSection = footerColumn ?? linkSection;
            }

            return View(linkSection);
        }

   }
}