CREATE PROCEDURE dbo.GetProductView
(
	@productid int
)
AS
SELECT 
	p.Id,
	p.[Name],
	p.ProductDetails,
	p.Specifications,
	p.ImageUrl,
	p.SmallImageUrl,
	p.OtherImageUrlsJson,
	p.Price,
	p.OldPrice,
	p.StoreId,
	p.CategoryId,
	c.[Name] as CategoryName,
	p.ReviewsCount,
	p.AverageRating,
	st.StoreName as VendorName,
	st.PhoneNumber as VendorPhoneNumber,
	st.StoreDescription as VendorDetails,
	st.CampusId as VendorLocationId,
	st.LogoPath as VendorLogoPath
FROM Products p (nolock)
JOIN Stores st (nolock) ON p.StoreId = st.Id
JOIN Categories c ON c.Id = p.CategoryId
WHERE p.Id = @ProductId