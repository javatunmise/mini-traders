
CREATE PROCEDURE Insert_ProductTags
(
	@ProductId int,
	@TagXml XML
)
AS
BEGIN

	declare @Tags as table(item varchar(50))
	insert into @Tags
	Select t.c.value('.','varchar(50)') as Item from @TagXml.nodes('//Tag') as t(c)


	INSERT INTO Tags(Name)
	(
		Select item From @Tags
		Except
		Select Name from Tags 
	)

	DELETE FROM ProductTags WHERE ProductId = @ProductId

	INSERT INTO ProductTags(ProductId, TagId)
	SELECT @ProductID, t.Id
	FROM @Tags tt JOIN Tags t ON t.Name = tt.Item
		
END