--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Deletes rows from the group level basis table based on store group level and method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GROUP_LEVEL_BASIS_DELETE] 
	@SGL_RID int,
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM GROUP_LEVEL_BASIS
	WHERE SGL_RID = @SGL_RID
		  AND METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO