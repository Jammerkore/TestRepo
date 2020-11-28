--dv =============================================
--dv Create date: 1/17/2014
--dv Description: Reads group level node functions for the method and store level group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GROUP_LEVEL_NODE_FUNCTION_READ] 
	@METHOD_RID int, 
	@SGL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HN_RID, 
		APPLY_MIN_MAXES_IND, 
		MIN_MAXES_INHERIT_TYPE
	FROM GROUP_LEVEL_NODE_FUNCTION 
	WHERE METHOD_RID = @METHOD_RID
		  AND SGL_RID = @SGL_RID

END
GO