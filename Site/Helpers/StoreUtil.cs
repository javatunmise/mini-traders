using site.Data;
using System.Collections.Generic;
using System.Linq;

namespace site.Helpers
{
    public static class StoreUtil
    {        
        public static bool IsServiceCategory(int currentCategoryId, List<Category> categories)
        {
            var category = categories.FirstOrDefault(e => e.Id == currentCategoryId);
            if (category == null) return false;
            if (category.Name == "Services") return true;
            if (category.Parent != null && category.Parent.Name == "Services") return true;
            if (category.Parent != null && category.Parent.Parent != null && category.Parent.Parent.Name == "Services") return true;

            return false;
        }
    }
}