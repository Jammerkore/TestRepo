--dv =============================================
--dv Create date: 1/17/2014
--dv Description: Deletes a group level node function
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GROUP_LEVEL_NODE_FUNCTION_DELETE] 
	@METHOD_RID int, 
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM GROUP_LEVEL_NODE_FUNCTION
    WHERE METHOD_RID =@METHOD_RID
          AND SGL_RID = @SGL_RID

	SELECT @@ROWCOUNT;

END
GO