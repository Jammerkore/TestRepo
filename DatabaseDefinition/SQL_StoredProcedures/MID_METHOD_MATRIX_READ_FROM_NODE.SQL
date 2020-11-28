--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Reads rows from the method matrix based on the provided hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MATRIX_READ_FROM_NODE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mm.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, 
		au.USER_NAME
	FROM METHOD_MATRIX mm, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE mm.HN_RID = @HN_RID
		  AND mm.METHOD_RID = m.METHOD_RID
		  AND m.USER_RID = au.USER_RID

END
GO