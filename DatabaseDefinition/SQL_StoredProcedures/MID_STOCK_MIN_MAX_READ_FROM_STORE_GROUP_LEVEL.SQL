--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Reads basis rows from the stock min max table based on method and store group level
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MIN_MAX_READ_FROM_STORE_GROUP_LEVEL] 
	@METHOD_RID int,
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		METHOD_RID, 
		SGL_RID, 
		BOUNDARY, 
		HN_RID, 
		CDR_RID, 
		MIN_STOCK, 
		MAX_STOCK
	FROM STOCK_MIN_MAX
	WHERE METHOD_RID = @METHOD_RID
		  AND SGL_RID = @SGL_RID
	ORDER BY HN_RID, BOUNDARY

END
GO