ALTER PROCEDURE dbo.GetProductReviews
	@ProductId int
AS
BEGIN
	SELECT TOP 1000 
		   p.[Id]
		  ,[ProductId]
		  ,[StoreId]
		  ,[Message]
		  ,[ReviewerId]
		  ,CASE WHEN ISNULL([ReviewerName],'') = '' THEN 'Unknown' ELSE ReviewerName END ReviewerName
		  ,[Rating]
		  ,[HideUserIdentity]
		  ,[CreatedOn],
		  COALESCE(u.ProfilePicturePath, 'images/vendor-photo.png') ReviewerProfileImage
	FROM [dbo].[ProductReviews] p
	JOIN SiteUsers u ON u.Id = p.ReviewerId
	WHERE p.ProductId = @ProductId
	ORDER BY p.CreatedOn DESC
END
