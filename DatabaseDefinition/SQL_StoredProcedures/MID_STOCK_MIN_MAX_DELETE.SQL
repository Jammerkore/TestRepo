--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Deletes stock min max basis based on method, store group level, and boundary
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MIN_MAX_DELETE] 
	@METHOD_RID int,
	@SGL_RID int,
	@BOUNDARY int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STOCK_MIN_MAX
	WHERE METHOD_RID = @METHOD_RID
		  AND SGL_RID = @SGL_RID
		  AND BOUNDARY = @BOUNDARY		  
	
	SELECT @@ROWCOUNT;

END
GO