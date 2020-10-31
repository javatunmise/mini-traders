CREATE PROCEDURE LatestProductsPROC
(
	@CategoryId int,
	@Top int = 10
)
AS
BEGIN
	-- Latest Products
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
		CampusId int,
		StoreLocation varchar(100))

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
		st.CampusId,
		c.Name as StoreLocation
	FROM Products p (nolock)
	JOIN Stores st (nolock) ON p.StoreId = st.Id
	JOIN Campuses c ON c.Id = st.CampusId
	WHERE 
		st.[Status] = 1 AND p.[Status] = 0 AND
		p.CategoryId IN (select id from @catids) --AND
		--(st.CampusId = @LocationId OR @LocationId IS NULL) AND
		--(st.HostelId = @SubLocationId OR @SubLocationId IS NULL)
	ORDER BY p.CreatedOn DESC

	Select TOP (@Top)

		p.StoreName,
		p.CampusId,
		p.Name as StoreLocation,
		pp.*
	From @table p
	JOIN Products pp ON p.Id = pp.Id
	Order By NewID()

END