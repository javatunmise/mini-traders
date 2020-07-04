using System;
using System.Collections.Generic;
using System.Text;

namespace SiteInfo
{
    public class SiteRepository
    {
        public Site Load()
        {
            return new Site
            {
                Address = new CompanyAddress
                {
                    PhoneNumber = "+234 813 776 8881",
                    Email = "shopatfirstchoice@yahoo.com",
                    Address = "159 Olonade Yaba, Lagos"
                },
                FacebookUrl = "http://facebook.com/pages/first-choice-online",
                GoogleUrl = "",
                InstagramUrl = "",
                TwitterUrl = "",
                LinkedInUrl = ""
            };
        }
    }
}
