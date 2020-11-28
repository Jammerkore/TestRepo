--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Inserts a basis row into the trend caps table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TREND_CAPS_INSERT] 
	@METHOD_RID int, 
	@SGL_RID int, 
	@TREND_CAP_ID int,
	@TOL_PCT float, 
	@HIGH_LIMIT float, 
	@LOW_LIMIT float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TREND_CAPS
		(
		METHOD_RID, 
		SGL_RID, 
		TREND_CAP_ID,
		TOL_PCT, 
		HIGH_LIMIT, 
		LOW_LIMIT
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID, 
		@TREND_CAP_ID,
		@TOL_PCT, 
		@HIGH_LIMIT, 
		@LOW_LIMIT
		)

	SELECT @@ROWCOUNT;

END
GO