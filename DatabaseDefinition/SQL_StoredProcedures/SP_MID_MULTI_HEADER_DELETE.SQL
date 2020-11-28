CREATE PROCEDURE [dbo].[SP_MID_MULTI_HEADER_DELETE] 
	(
	@HDR_RID INT,
	@debug BIT = 0,
	@HEADER_DELETE_COUNT INT OUTPUT
	)
AS
BEGIN
	DECLARE @purgeHeaders BIT

	SELECT @purgeHeaders = 1 -- change this to 1 when ready to purge 

	SELECT @HEADER_DELETE_COUNT = 0

	-- get header to purge 
	DECLARE @h TABLE (
		hdrRID INT NULL,
		hdrID VARCHAR(100) NULL,
		hdrRelDt DATETIME NULL,
		hdrShipDt DATETIME NULL,
		hdrStyleRID INT NULL,
		hdrGroupRID INT NULL
		)

	INSERT @h (
		hdrRID,
		hdrID,
		hdrRelDt,
		hdrShipDt,
		hdrStyleRID,
		hdrGroupRID
		)
	SELECT HDR_RID,
		HDR_ID,
		RELEASE_DATETIME,
		SHIP_TO_DAY,
		STYLE_HNRID,
		HDR_GROUP_RID
	FROM VW_GET_HEADERS vw
	WHERE vw.HDR_RID = @HDR_RID

	IF (@debug <> 0)
		SELECT *
		FROM @h

	DECLARE @hRID INT
	-- check status of children and remove all multi-headers where all children are not shipping complete
	DECLARE @h2 TABLE (hdrRID INT NULL)

	INSERT @h2 (hdrRID)
	SELECT hdrRID
	FROM @h

	DECLARE @childCount INT,
		@shippedCount INT

	SELECT TOP 1 @hRID = hdrRID
	FROM @h2

	WHILE @@rowcount <> 0
	BEGIN
		SELECT @childCount = count(*)
		FROM VW_GET_HEADERS
		WHERE HDR_GROUP_RID = @hRID

		SELECT @shippedCount = count(*)
		FROM VW_GET_HEADERS
		WHERE HDR_GROUP_RID = @hRID
			AND ShippingComplete = '1'

		IF (@shippedCount != @childCount) -- remove header if all children not shipped
		BEGIN
			DELETE @h
			WHERE hdrRID = @hRID
		END

		DELETE @h2
		WHERE hdrRID = @hRID

		SELECT TOP 1 @hRID = hdrRID
		FROM @h2
	END

	IF (@debug <> 0)
		SELECT *
		FROM @h

	---- add children to delete list 
	INSERT @h (
		hdrRID,
		hdrID,
		hdrRelDt,
		hdrShipDt,
		hdrStyleRID,
		hdrGroupRID
		)
	SELECT HDR_RID,
		HDR_ID,
		RELEASE_DATETIME,
		SHIP_TO_DAY,
		STYLE_HNRID,
		HDR_GROUP_RID
	FROM VW_GET_HEADERS
	WHERE HDR_GROUP_RID IN (
			SELECT hdrRID
			FROM @h
			)

	-- create table to remove relationships
	DECLARE @h3 TABLE (hdrRID INT NULL)

	INSERT @h3 (hdrRID)
	SELECT hdrRID
	FROM @h

	---- remove data relationships
	IF (@purgeHeaders <> 0)
	BEGIN
		SELECT TOP 1 @hRID = hdrRID
		FROM @h3

		WHILE @@rowcount <> 0
		BEGIN
			-- remove the group header RID from the children
			UPDATE HEADER
			WITH (ROWLOCK)

			SET HDR_GROUP_RID = 1
			WHERE HDR_GROUP_RID = @hRID

			-- remove all pack relationships from the multi
			DECLARE @t TABLE (HP INT)

			INSERT INTO @t
			SELECT HDR_PACK_RID
			FROM HEADER_PACK
			WHERE HDR_RID = @hRID

			DECLARE @HP INT

			SELECT TOP 1 @HP = HP
			FROM @t

			WHILE @@rowcount <> 0
			BEGIN
				UPDATE HEADER_PACK
				WITH (ROWLOCK)

				SET ASSOCIATED_PACK_RID = NULL
				WHERE HDR_PACK_RID = @HP

				DELETE @t
				WHERE HP = @HP

				SELECT TOP 1 @HP = HP
				FROM @t
			END

			DELETE @h3
			WHERE hdrRID = @hRID

			SELECT TOP 1 @hRID = hdrRID
			FROM @h3
		END
	END

	-- delete headers
	-- show list of headers to purge
	IF (@debug <> 0)
		SELECT *
		FROM @h

	IF (@purgeHeaders <> 0)
	BEGIN
		SELECT @HEADER_DELETE_COUNT = count(*)
		FROM @h

		SELECT TOP 1 @hRID = hdrRID
		FROM @h

		WHILE @@rowcount <> 0
		BEGIN
			-- print @hRID
			EXEC dbo.SP_MID_HEADER_DELETE @hRID, 0

			DELETE @h
			WHERE hdrRID = @hRID

			SELECT TOP 1 @hRID = hdrRID
			FROM @h
		END
	END
END
GO


