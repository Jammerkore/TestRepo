--dv =============================================
--dv Create date: 12/10/2013
--dv Description: Reads an export method from the hierarchy node provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_EXPORT_READ_FROM_NODE] 
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mcf.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID, 
		m.USER_RID, 
		au.USER_NAME
	FROM METHOD_EXPORT mcf, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE mcf.HN_RID = @HN_RID 
		  AND mcf.METHOD_RID = m.METHOD_RID 
		  AND m.USER_RID = au.USER_RID

END
GO