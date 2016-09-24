/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.5.278
 * Time: 24.09.2016 15:57:15
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
	@cLength FLOAT,
	@comment NVARCHAR(400),
	@adminComment NVARCHAR(400)
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
	    CargoLength,
	    Comment,
	    AdminComment
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
	    @cLength,
	    @comment,
	    @adminComment
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
	@cLength FLOAT,
	@comment NVARCHAR(400),
	@adminComment NVARCHAR(400),
	@status INT
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
	       CargoLength = @cLength,
	       Comment = @comment,
	       AdminComment = @adminComment,
	       [Status] = @status
	WHERE  Id = @id
	
	EXEC dbo.get_order @id
END
GO

/*******************************************
 * изменить статус заявки
 *******************************************/
IF OBJECT_ID(N'dbo.change_order_status', N'P') IS NOT NULL
    DROP PROCEDURE dbo.change_order_status;
GO
CREATE PROCEDURE dbo.change_order_status
	@orderId INT,
	@status INT
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	UPDATE D_ORDER
	SET    [Status]       = @status,
	       DateUpdate     = @date
	WHERE  Id             = @orderId
END
GO

/*******************************************
 * забрать груз водителем
 *******************************************/
IF OBJECT_ID(N'dbo.take_cargo', N'P') IS NOT NULL
    DROP PROCEDURE dbo.take_cargo;
GO
CREATE PROCEDURE dbo.take_cargo
	@orderId INT,
	@userId NVARCHAR(128)
AS
BEGIN
	DECLARE @date DATETIME = GETDATE();
	
	IF NOT EXISTS(
	       SELECT TOP 1 dot.OrderId
	       FROM   D_ORDER_TRACK AS dot
	       WHERE  dot.OrderId = @orderId
	              AND dot.UserId = dot.UserId
	              AND IsActive = 1
	   )
	BEGIN
	    INSERT INTO D_ORDER_TRACK
	      (
	        OrderId,
	        UserId,
	        DateCreate,
	        DateUpdtae,
	        IsActive
	      )
	    VALUES
	      (
	        @orderId,
	        @userId,
	        @date,
	        @date,
	        1
	      )
	    
	    UPDATE D_ORDER
	    SET    [Status]     = 2
	    WHERE  Id           = @orderId
	END
END
GO

/*******************************************
 * отменить забор грузa водителем
 *******************************************/
IF OBJECT_ID(N'dbo.untake_cargo', N'P') IS NOT NULL
    DROP PROCEDURE dbo.untake_cargo;
GO
CREATE PROCEDURE dbo.untake_cargo
	@orderId INT
AS
BEGIN
	UPDATE D_ORDER_TRACK
	SET    IsActive = 0
	WHERE  OrderId = @orderId
	
	UPDATE D_ORDER
	SET    [Status]     = 1	--принято в работу
	WHERE  Id           = @orderId
END
GO