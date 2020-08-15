using site.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace site.Helpers
{
    public static class PathUtil
    {
        public static string GetCategoryPath(Category category) => $"/categories/{category.Id}/{UrlEncoder.Default.Encode(category.Name)}";
    }
}
