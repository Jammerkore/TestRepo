--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads general allocation methods by merchandise hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GENERAL_ALLOCATION_READ_FROM_NODE] 
	@MERCH_HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mga.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, au.USER_NAME
	FROM METHOD_GENERAL_ALLOCATION mga, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE mga.MERCH_HN_RID = @MERCH_HN_RID
		  AND mga.METHOD_RID = m.METHOD_RID
		  AND m.USER_RID = au.USER_RID
	 
END
GO










