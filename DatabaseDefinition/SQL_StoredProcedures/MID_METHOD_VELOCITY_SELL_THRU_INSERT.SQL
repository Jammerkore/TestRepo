--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Inserts a velocity method sell thru
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_SELL_THRU_INSERT]
	@METHOD_RID int,
	@VELOCITY_SELL_THRU_INDEX int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_VELOCITY_SELL_THRU  
		(
		METHOD_RID, 
		VELOCITY_SELL_THRU_INDEX
		)
	VALUES
		(
		@METHOD_RID, 
		@VELOCITY_SELL_THRU_INDEX
		)

	SELECT @@ROWCOUNT;
END
GO