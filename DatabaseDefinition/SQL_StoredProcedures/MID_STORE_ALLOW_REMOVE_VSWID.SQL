CREATE PROCEDURE [dbo].[MID_STORE_ALLOW_REMOVE_VSWID] 
	@ST_RID INT,
	@ReturnCode INT OUTPUT
AS
BEGIN
	SET @ReturnCode = 1  /* Default to allow remove */
	DECLARE @IMO_ID VARCHAR(50)

	/* Procedure checks to see if the given SCG_RID is in use in tables */
	/* if so,  we can not delete the size curve group                                  */
	DECLARE @rowsAffected INT

	/* TOTAL_ALLOCATION */
	SELECT TOP 1 ITEM_UNITS_ALLOCATED
	FROM TOTAL_ALLOCATION with (nolock)
	WHERE ST_RID = @ST_RID
	  AND ITEM_UNITS_ALLOCATED > 0

	SELECT @rowsAffected = @@rowcount

	IF @rowsAffected > 0
		SET @ReturnCode = 0

	IF @ReturnCode = 1
	BEGIN
		/*  CAN REMOVE ID FROM LAST STORE IF NO VSW HEADER */
		SELECT @IMO_ID = IMO_ID
		FROM STORES
		WHERE ST_RID = @ST_RID

		SELECT @rowsAffected = count(*)
		FROM STORES
		WHERE IMO_ID = @IMO_ID

		IF @rowsAffected = 1
		BEGIN
		    SELECT TOP 1 IMO_ID
	        FROM HEADER with (nolock)
	        WHERE IMO_ID = @IMO_ID

			SELECT @rowsAffected = @@rowcount

			IF @rowsAffected > 0
			   SET @ReturnCode = 0
		END
	END

END
GO