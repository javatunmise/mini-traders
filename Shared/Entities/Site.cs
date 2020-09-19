using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class Site
    {
        public static string Identifier = "F93CF999-F290-45D0-B619-83157DD9AB35";

        public Guid Id { get; set; }
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
