--dv =============================================
--dv Create date: 2/23/2018
--dv Description: Reads a planning extract method from the hierarchy node provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_PLANNING_EXTRACT_READ_FROM_NODE] 
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mpe.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID, 
		m.USER_RID, 
		au.USER_NAME
	FROM METHOD_PLANNING_EXTRACT mpe, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE mpe.HN_RID = @HN_RID 
		  AND mpe.METHOD_RID = m.METHOD_RID 
		  AND m.USER_RID = au.USER_RID

END
GO