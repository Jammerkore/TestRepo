--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads a stock min max row for the hierarchy node rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_STOCK_MIN_MAX_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_RID, 
		SGL_RID, 
		BOUNDARY, 
		CDR_RID, 
		MIN_STOCK, 
		MAX_STOCK																										 
	FROM NODE_STOCK_MIN_MAX
	WHERE HN_RID = @HN_RID
	ORDER BY SGL_RID, BOUNDARY

END
GO
