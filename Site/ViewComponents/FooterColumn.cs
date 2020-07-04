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

        public async Task<IViewComponentResult> InvokeAsync(string section)
        {
            await Task.CompletedTask;
            var model = new FooterLinks(section, new List<FooterLink>());

            if (section == "Categories")
            {
                var categories = (await _provider.GetAllCategories())
                     .Where(c => c.Parent == null)
                     .Select(c => new FooterLink(c.Name, Util.GetCategorySlug(c)));

                model = new FooterLinks("Categories", categories);
            }

            return View(model);
        }

        public class FooterLinks
        {
            public FooterLinks(string title, IEnumerable<FooterLink> links)
            {
                Title = title;
                Links = links;
            }

            public string Title { get; private set; }
            public IEnumerable<FooterLink> Links { get; private set; }
        }

        public class FooterLink
        {
            public FooterLink(string text, string url)
            {
                Text = text;
                Url = url;
            }

            public string Text { get; set; }
            public string Url { get; set; }
        }
   }

}
