--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Inserts a basis row into the stock min max table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MIN_MAX_INSERT] 
	@METHOD_RID int, 
	@SGL_RID int, 
	@BOUNDARY int, 
	@HN_RID int, 
	@CDR_RID int, 
	@MIN_STOCK int, 
	@MAX_STOCK int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO STOCK_MIN_MAX
		(
		METHOD_RID, 
		SGL_RID, 
		BOUNDARY, 
		HN_RID, 
		CDR_RID, 
		MIN_STOCK, 
		MAX_STOCK
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID, 
		@BOUNDARY, 
		@HN_RID, 
		@CDR_RID, 
		@MIN_STOCK, 
		@MAX_STOCK
		)

	SELECT @@ROWCOUNT;

END
GO