--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Deletes stock min max basis based on method, store group level, and hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MIN_MAX_DELETE_FROM_NODE] 
	@METHOD_RID int,
	@SGL_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STOCK_MIN_MAX
	WHERE METHOD_RID = @METHOD_RID
		  AND SGL_RID = @SGL_RID		  
	      AND HN_RID = @HN_RID

	SELECT @@ROWCOUNT;

END
GO