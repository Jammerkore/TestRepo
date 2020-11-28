--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Deletes group level basis based on method and store group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GROUP_LEVEL_BASIS_DELETE_FROM_GROUP] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM GROUP_LEVEL_BASIS
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO