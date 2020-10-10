using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;
using Site.Data;

namespace Site.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public bool ShowGoogleAds { get; private set;}
        public SiteImage TopBannerFileName { get; private set;}
        public SiteImage MiddleBanner1FileName { get; private set;}
        public SiteImage MiddleBanner2FileName { get; private set;}
        public SiteImage BannerAfterMiddleFileName { get; private set;}

        public IndexModel(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task OnGet()
        {
            var banners = await _dbContext.SiteImages.Where(e => e.GroupCode == ImageUploadGroups.Banners)
                                .ToListAsync();

            ShowGoogleAds = false;
            TopBannerFileName = GetBannerImageUrl(banners, SiteImage.BannerPositions[0], "top-banner.png");
            MiddleBanner1FileName = GetBannerImageUrl(banners, SiteImage.BannerPositions[1], "CONFORT.png");
            MiddleBanner2FileName = GetBannerImageUrl(banners, SiteImage.BannerPositions[2], "Few Second.png");
            BannerAfterMiddleFileName = GetBannerImageUrl(banners, SiteImage.BannerPositions[3], "banner-after-middle.jpg");
        }

        private SiteImage GetBannerImageUrl(List<SiteImage> banners, string bannerPosition, string defaultIfNotFound)
        {
            var banner = banners.FirstOrDefault(e => e.EntityId == bannerPosition);

            return banner ?? new SiteImage { ImagePath = $"images/banners/{defaultIfNotFound}", Link = "" };
        }
    }
}
