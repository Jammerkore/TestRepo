--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Inserts a velocity matrix
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_MATRIX_INSERT]
	@METHOD_RID int,
	@SGL_RID int,
	@BOUNDARY int, 
	@VELOCITY_SELL_THRU_INDEX int,
	@VELOCITY_RULE int,
	@VELOCITY_QUANTITY float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_VELOCITY_MATRIX 
		(
		METHOD_RID, 
		SGL_RID, 
		BOUNDARY, 
		VELOCITY_SELL_THRU_INDEX,
		VELOCITY_RULE,
		VELOCITY_QUANTITY
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID, 
		@BOUNDARY, 
		@VELOCITY_SELL_THRU_INDEX,
		@VELOCITY_RULE,
		@VELOCITY_QUANTITY
		)

	SELECT @@ROWCOUNT;
END
GO