using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Entities
{
    public class SiteLink
    {
        public SiteLink()
        {
            Target = "_self";
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public int Ordering { get; set; }
        public LinkGroups LinkGroup { get; set; }

        public enum LinkGroups
        {
            FooterLinkHelpAndSupport = 1,
            FooterLinkCustomerService = 2,
            FooterLinkCorporation = 3,
            FooterLinkWhyChoseUs  = 4
        }

        public const string HELP_AND_SUPPORT = "HELP_AND_SUPPORT";
        public const string CUSTOMER_SERVICE = "CUSTOMER_SERVICE";
        public const string CORPORATION = "CORPORATION";
        public const string WHY_CHOOSE_US = "WHY_CHOOSE_US";

    }
}
