--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Inserts a stock min max row 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_STOCK_MIN_MAX_INSERT]
	@HN_RID int,
	@SGL_RID int,
	@BOUNDARY int,
	@CDR_RID int,
	@MIN_STOCK int,
	@MAX_STOCK int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO NODE_STOCK_MIN_MAX(HN_RID, SGL_RID, BOUNDARY, CDR_RID, MIN_STOCK, MAX_STOCK)																									
	VALUES (@HN_RID, @SGL_RID, @BOUNDARY, @CDR_RID, @MIN_STOCK, @MAX_STOCK)

	SELECT @@ROWCOUNT;

END
GO
