CREATE PROCEDURE [dbo].[SP_MID_IS_SIZE_CURVE_GROUP_IN_USE] 
	@SCG_RID INT,
	@ReturnCode INT OUTPUT
AS
BEGIN
	SET @ReturnCode = 0

	/* Procedure checks to see if the given SCG_RID is in use in tables */
	/* if so,  we can not delete the size curve group                                  */
	DECLARE @rowsAffected INT

	/* SIZE_CONSTRAINT_MODEL */
	SELECT TOP 1 SIZE_CURVE_GROUP_RID
	FROM SIZE_CONSTRAINT_MODEL
	WHERE SIZE_CURVE_GROUP_RID = @SCG_RID

	SELECT @rowsAffected = @@rowcount

	IF @rowsAffected > 0
		SET @ReturnCode = 1

	IF @ReturnCode = 0
	BEGIN
		/*  METHOD_FILL_SIZE_HOLES */
		SELECT TOP 1 SIZE_CURVE_GROUP_RID
		FROM METHOD_FILL_SIZE_HOLES
		WHERE SIZE_CURVE_GROUP_RID = @SCG_RID

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	IF @ReturnCode = 0
	BEGIN
		/* METHOD_SIZE_NEED */
		SELECT TOP 1 SIZE_CURVE_GROUP_RID
		FROM METHOD_SIZE_NEED
		WHERE SIZE_CURVE_GROUP_RID = @SCG_RID

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	IF @ReturnCode = 0
	BEGIN
		/* METHOD_SIZE_NEED_ALLOCATION */
		SELECT TOP 1 SIZE_CURVE_GROUP_RID
		FROM METHOD_SIZE_NEED_ALLOCATION
		WHERE SIZE_CURVE_GROUP_RID = @SCG_RID

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	IF @ReturnCode = 0
	BEGIN
		/* METHOD_SIZE_BASIS_ALLOCATION */
		SELECT TOP 1 SIZE_CURVE_GROUP_RID
		FROM METHOD_SIZE_BASIS_ALLOCATION
		WHERE SIZE_CURVE_GROUP_RID = @SCG_RID

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	IF @ReturnCode = 0
	BEGIN
		/* HEADER_SIZE_NEED */
		SELECT TOP 1 SIZE_CURVE_GROUP_RID
		FROM HEADER_SIZE_NEED
		WHERE SIZE_CURVE_GROUP_RID = @SCG_RID

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END

	IF @ReturnCode = 0
	BEGIN
		/* HEADER_BULK_COLOR_SIZE_NEED */
		SELECT TOP 1 SIZE_CURVE_GROUP_RID
		FROM HEADER_BULK_COLOR_SIZE_NEED
		WHERE SIZE_CURVE_GROUP_RID = @SCG_RID

		SELECT @rowsAffected = @@rowcount

		IF @rowsAffected > 0
			SET @ReturnCode = 1
	END
END
GO


