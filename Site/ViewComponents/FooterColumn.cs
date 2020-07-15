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
        private readonly ISiteContentProvider _provider;

        public FooterColumn(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync(FooterColumnIndex section)
        {
            await Task.CompletedTask;
            var linkSection = new FooterLinks("", new List<FooterLink>());

            if (section == FooterColumnIndex.FirstColumn)
            {
                var footerColumn = await _provider.GetFooterFirstLinkSection();

                linkSection = footerColumn ?? linkSection;
            }
            if (section == FooterColumnIndex.SecondColumn)
            {
                var footerColumn = await _provider.GetFooterSecondLinkSection();

                linkSection = footerColumn ?? linkSection;
            }

            return View(linkSection);
        }

   }

    public enum FooterColumnIndex
    {
        FirstColumn, SecondColumn
    }
}