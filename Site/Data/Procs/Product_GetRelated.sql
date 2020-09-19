CREATE PROCEDURE Product_GetRelated
(
	@TagsXml XML
)
AS
BEGIN

	declare @Tags as table(item varchar(50))
	insert into @Tags
	Select t.c.value('.','varchar(50)') as Item from @TagsXml.nodes('//Tag') as t(c)


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
		p.ReviewsCount,
		p.AverageRating
	FROM @Tags tt
	JOIN Tags t ON t.Name = tt.item
	JOIN ProductTags pt ON pt.TagId = t.Id
	JOIN Products p ON p.Id = pt.ProductId
	GROUP BY
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
		p.ReviewsCount,
		p.AverageRating

END