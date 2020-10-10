ALTER PROCEDURE SearchProduct
(
	@SearchText varchar(50) = NULL,
	@LocationId int = NULL,
	@SubLocationId int = NULL,
	@CategoryId int = NULL,
	@PriceMin int = 0,
	@PriceMax int = NULL,
	@PageIndex int = 1,
	@PageSize int = 20,
	@Sort varchar(20) = 'date'
)
AS
DECLARE
	@StartIndex int,
	@EndIndex int

	Select @StartIndex = ((@PageIndex - 1) * @PageSize)+  1, @EndIndex = (@PageIndex * @PageSize)

DECLARE @table as table(Id int,
	Name varchar(128),
	ImageUrl varchar(255),
	SmallImageUrl varchar(255),
	Price decimal(18,2),
	OldPrice decimal(18,2),
	StoreId int,
	CategoryId int,
	AverageRating float,
	CreatedOn datetime2,
	StoreName varchar(128),
	CampusId int)

declare @catids as table(id int)

IF @CategoryId IS NOT NULL
BEGIN
	;WITH MyCTE AS 
	(
		SELECT Id, Name, ISNULL(ParentId, Id) ParentId
		FROM Categories
		WHERE Id = @categoryid 
		UNION ALL
		SELECT c.Id, c.Name, c.ParentId
		FROM Categories c
		INNER JOIN MyCTE ON c.ParentId = MyCTE.Id
		WHERE c.Id != @categoryid
	)
	INSERT INTO @catids
	SELECT DISTINCT Id FROM MyCTE
END


INSERT INTO @table
SELECT 
	p.Id,
	p.Name,
	p.ImageUrl,
	p.SmallImageUrl,
	p.Price,
	p.OldPrice,
	p.StoreId,
	p.CategoryId,
	p.AverageRating,
	p.CreatedOn,
	st.StoreName,
	st.CampusId
FROM Products p (nolock)
JOIN Stores st (nolock) ON p.StoreId = st.Id
WHERE 
	(@SearchText IS NULL OR p.Name LIKE @SearchText + '%') AND
	(@CategoryId IS NULL OR p.CategoryId IN (select id from @catids)) AND
	(p.Price >= @PriceMin AND (@PriceMax IS NULL OR p.Price <= @PriceMax)) AND
	(st.CampusId = @LocationId OR @LocationId IS NULL) AND
	(st.HostelId = @SubLocationId OR @SubLocationId IS NULL)

SELECT MAX(p.Price) MaxPrice, MIN(p.Price) MinPrice, Count(1) RecordCount FROM @table p

SELECT * FROM (
	SELECT
	p.Id,
	p.Name,
	p.ImageUrl,
	p.SmallImageUrl,
	p.Price,
	p.OldPrice,
	p.StoreId,
	p.CategoryId,
	p.AverageRating,
	p.StoreName,
	p.CampusId,
	ROW_NUMBER() OVER (ORDER BY 
			CASE WHEN @Sort = 'date' THEN p.CreatedOn END DESC,
			CASE WHEN @Sort = 'price-desc' THEN p.Price END DESC,
			CASE WHEN @Sort = 'price' THEN p.Price END) rowNum 
 FROM @table p) p
 WHERE RowNum BETWEEN @StartIndex AND @EndIndex