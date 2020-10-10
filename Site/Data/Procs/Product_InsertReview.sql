CREATE PROCEDURE Product_InsertReview
(
	@productId int, 
	@storeid int, 
	@message varchar(1024), 
	@reviewerid int, 
	@reviewername varchar(100), 
	@rating float,
	@hideUserIdentity bit, 
	@createdon datetime
)
AS

BEGIN

	DECLARE @reviewCount float, @totalRating float, @averageRating float

	SELECT @reviewCount = COUNT(1), @totalRating = SUM(r.Rating) FROM ProductReviews r
	WHERE r.ProductId = @ProductId

	SELECT @averageRating = @totalRating / @reviewCount

	INSERT INTO ProductReviews
	(productId,storeId,[message],reviewerId,reviewerName,rating,hideUserIdentity,createdOn) 
	VALUES 
	(@productId, @storeid, @message, @reviewerid, @reviewername, @rating, @hideUserIdentity, @createdon);


	UPDATE dbo.Products SET 
		AverageRating = COALESCE(@averageRating, 0), 
		ReviewsCount = @reviewCount
	WHERE Id = @ProductId



END