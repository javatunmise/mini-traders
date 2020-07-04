using Microsoft.AspNetCore.Mvc;
using site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class GroupsGrid : ViewComponent
    {
        private readonly ISiteContentProvider _provider;

        public GroupsGrid(ISiteContentProvider provider)
        {
            _provider = provider;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName)
        {
            var model = new GroupsGridInfo();
            var view = (string.IsNullOrWhiteSpace(viewName) ? "" : viewName).ToLower();

            if (view == "top-categories")
            {
                var data = await _provider.GetSiteTopCategories();
                SetupTopCategories(model, data);
            }
            else if(view == "our-partners")
            {
                var data = await _provider.GetSitePartnersLogos();
                SetupOurPartners(model, data);
            }

            return View(model);
        }

        private void SetupTopCategories(GroupsGridInfo model, IEnumerable<Category> data)
        {
            model.Title = "Top Categories";
            model.ViewData = data.Select(i => new ImageAndUrl
            {
                DestinationUrl = $"/categories/{i.Id}-{i.Name}",
                ImageUrl = string.IsNullOrWhiteSpace(i.IconClass) ? "cart-plus" : i.IconClass,
                Text = i.Name
            });
        }

        private void SetupOurPartners(GroupsGridInfo model, IEnumerable<CompanyLogo> data)
        {
            model.Title = "Our Partners";
            model.UseImages = true;
            model.ViewData = data.Select(i => new ImageAndUrl
            {
                DestinationUrl = i.CompanySiteUrl,
                ImageUrl = string.IsNullOrWhiteSpace(i.CompanyLogoUrl) ? "#" : i.CompanyLogoUrl,
                Text = ""
            });
        }

        public class GroupsGridInfo
        {
            public GroupsGridInfo()
            {
                ViewData = new List<ImageAndUrl>();
                Title = "No Data";
            }

            public string Title { get; set; }
            public IEnumerable<ImageAndUrl> ViewData { get; set; }
            public bool UseImages { get; internal set; }
        }
    }

    public class ImageAndUrl
    {
        public string DestinationUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Text { get; set; }
    }
}
