using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Site.Pages
{
    public class IndexModel : PageModel
    {
        public bool ShowGoogleAds { get; }
        public string TopBannerFileName { get; }
        public string MiddleBanner1FileName { get; }
        public string MiddleBanner2FileName { get; }
        public string BannerAfterMiddleFileName { get; }

        public IndexModel()
        {
            ShowGoogleAds = false;
            TopBannerFileName = "top-banner.png";
            //MiddleBanner1FileName = "home-banner1.jpg";
            //MiddleBanner2FileName = "home-banner3.jpg";
            MiddleBanner1FileName = "CONFORT.png";
            MiddleBanner2FileName = "Few Second.png";

            BannerAfterMiddleFileName = "banner-after-middle.jpg";
        }

        public void OnGet()
        {
        }
    }
}
