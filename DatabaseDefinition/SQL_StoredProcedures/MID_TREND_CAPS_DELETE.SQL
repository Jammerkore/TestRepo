--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Deletes basis rows from the trend caps table based on method and store group level
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TREND_CAPS_DELETE] 
	@METHOD_RID int,
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM TREND_CAPS
	WHERE METHOD_RID = @METHOD_RID
		  AND SGL_RID = @SGL_RID

	SELECT @@ROWCOUNT;

END
GO