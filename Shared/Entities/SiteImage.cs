using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class SiteImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public ImageUploadGroups GroupCode { get; set; }
        public string EntityId { get; set; }
        public int Ordering { get; set; }
        public string Link { get; set; }


        public static List<string> BannerPositions =>
            new List<string> { "TopBanner", "MiddleBanner-1",
                "MiddleBanner-2", "MiddleBanner-3" };
    }


    public enum ImageUploadGroups
    {
        Banners, Carousels
    }
}
