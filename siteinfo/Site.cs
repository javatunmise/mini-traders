using System;

namespace SiteInfo
{
    public class Site
    {


        public CompanyAddress Address { get; set; }
        public string FacebookUrl { get; internal set; }
        public string GoogleUrl { get; internal set; }
        public string InstagramUrl { get; internal set; }
        public string LinkedInUrl { get; internal set; }
        public string TwitterUrl { get; internal set; }
    }
}
