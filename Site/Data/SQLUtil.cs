using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data
{
    public static class SQLUtil
    {
		public static string ProductColumns => "p.*";
		//	@"
		//p.Id,
		//p.[Name],
		//p.ProductDetails,
		//p.Specifications,
		//p.ImageUrl,
		//p.SmallImageUrl,
		//p.OtherImageUrlsJson,
		//p.Price,
		//p.OldPrice,
		//p.StoreId,
		//p.CategoryId,
		//p.ReviewsCount,
		//p.AverageRating";

		internal static string GetTopServicesQuery()
		{
			return
			(@$"
				SELECT
					TOP 10
					{ProductColumns}
				FROM Products p
				JOIN Stores st ON st.Id = p.StoreId
				WHERE st.Status=1 AND p.Status=1 AND p.RenderedAsService = 1
				ORDER BY NEWID()
			");
		}

		internal static string GetRecommendedQuery()
        {
			return
			(@$"
				SELECT
					TOP 10
					{ProductColumns}
				FROM Products p
				JOIN Stores st ON st.Id = p.StoreId
				WHERE (@CampusId = 0 OR st.CampusId = @CampusId) AND st.Status=1 AND p.Status=1
				ORDER BY NEWID()
			");
		}
	}
}
