/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 21.09.2016 17:14:38
 ************************************************************/

/*******************************************
 * получить заявку
 *******************************************/
IF OBJECT_ID(N'dbo.get_order', N'P') IS NOT NULL
    DROP PROCEDURE dbo.get_order;
GO
CREATE PROCEDURE dbo.get_order
	@id INT
AS
BEGIN
	SELECT *
	FROM   D_ORDER AS do
	WHERE  do.Id = @id
END
GO

/*******************************************
 * добавить заявку
 *******************************************/
IF OBJECT_ID(N'dbo.add_order', N'P') IS NOT NULL
    DROP PROCEDURE dbo.add_order;
GO
CREATE PROCEDURE dbo.add_order
	@df NVARCHAR(150),
	@dt NVARCHAR(150),
	@tdb DATETIME,
	@tde DATETIME,
	@uid NVARCHAR(128),
	@cName NVARCHAR(150),
	@cWeight FLOAT,
	@cWidth FLOAT,
	@cHeight FLOAT,
	@cLength FLOAT
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	INSERT INTO D_ORDER
	  (
	    DestinationFrom,
	    DestinationTo,
	    TakeDateBegin,
	    TakeDateEnd,
	    DateUpdate,
	    DateCreate,
	    UserId,
	    CargoName,
	    CargoWeight,
	    CargoWidth,
	    CargoHeight,
	    CargoLength
	  )
	VALUES
	  (
	    @df,
	    @dt,
	    @tdb,
	    @tde,
	    @date,
	    @date,
	    @uid,
	    @cName,
	    @cWeight,
	    @cWidth,
	    @cHeight,
	    @cLength
	  )
	
	DECLARE @id INT
	SELECT @id = @@identity
	
	EXEC dbo.get_order @id
END
GO

/*******************************************
 * обновить заявку
 *******************************************/
IF OBJECT_ID(N'dbo.update_order', N'P') IS NOT NULL
    DROP PROCEDURE dbo.update_order;
GO
CREATE PROCEDURE dbo.update_order
	@id INT,
	@df NVARCHAR(150),
	@dt NVARCHAR(150),
	@tdb DATETIME,
	@tde DATETIME,
	@cName NVARCHAR(150),
	@cWeight FLOAT,
	@cWidth FLOAT,
	@cHeight FLOAT,
	@cLength FLOAT
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	UPDATE D_ORDER
	SET    DestinationFrom = @df,
	       DestinationTo = @dt,
	       TakeDateBegin = @tdb,
	       TakeDateEnd = @tde,
	       DateUpdate = @date,
	       CargoName = @cName,
	       CargoWeight = @cWeight,
	       CargoWidth = @cWidth,
	       CargoHeight = @cHeight,
	       CargoLength = @cLength
	WHERE  Id = @id
	
	EXEC dbo.get_order @id
END
GO