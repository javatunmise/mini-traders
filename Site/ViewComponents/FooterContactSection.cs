using Microsoft.AspNetCore.Mvc;
using site.Data;
using System.Threading.Tasks;

namespace site.ViewComponents
{
    public class FooterContactSection : ViewComponent
    {
        private readonly ISiteContentProvider provider;

        public FooterContactSection(ISiteContentProvider provider)
        {
            this.provider = provider;
        }
       
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var site = await provider.GetSiteInfo();
            return View(site ?? new site.Data.Site());
        }
    }
}
