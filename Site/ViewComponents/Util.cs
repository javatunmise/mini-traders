using System;
using site.Data;

namespace site.ViewComponents
{
    internal class Util
    {
        internal static string GetCategorySlug(Category c)
        {
            return $"/categories/{c.Id}-{c.Name}";
        }
    }
}