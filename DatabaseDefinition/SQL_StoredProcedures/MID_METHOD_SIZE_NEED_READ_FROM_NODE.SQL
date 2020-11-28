--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads methods for the node for the size need plan
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_NEED_READ_FROM_NODE]
	@MERCH_HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		msn.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, 
		au.USER_NAME
	FROM METHOD_SIZE_NEED msn, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE msn.MERCH_HN_RID = @MERCH_HN_RID
		  AND msn.METHOD_RID = m.METHOD_RID 
		  AND m.USER_RID = au.USER_RID
            
END
GO