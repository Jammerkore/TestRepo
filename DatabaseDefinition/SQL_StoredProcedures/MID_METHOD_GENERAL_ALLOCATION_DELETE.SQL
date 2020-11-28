--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Deletes a general allocation method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GENERAL_ALLOCATION_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_GENERAL_ALLOCATION
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
	 
END
GO









