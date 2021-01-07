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
        public static string GetCategoryPath(Category category)
        {
            return $"/categories/{category.Id}/{UrlEncoder.Default.Encode(category.Name.Replace("/", "|"))}";
        }

        public static string GetProductPath(Shared.Entities.Product product)
        {
            return $"/products/{product.Id}-{UrlEncoder.Default.Encode(product.Name)}";
        }

        public static string Escape(string path)
        {
            return path.TrimStart('/');
        }

        public static string EscapeUrl(string path)
        {
            return UrlEncoder.Default.Encode(path);
        }
    }
}
