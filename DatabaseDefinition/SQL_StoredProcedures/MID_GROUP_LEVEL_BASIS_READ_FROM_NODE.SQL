--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Reads group level basis by hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GROUP_LEVEL_BASIS_READ_FROM_NODE] 
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		glb.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, 
		au.USER_NAME
	FROM GROUP_LEVEL_BASIS glb, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE glb.HN_RID = @HN_RID
		  AND glb.METHOD_RID = m.METHOD_RID
		  AND m.USER_RID = au.USER_RID

END
GO