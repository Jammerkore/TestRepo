--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads methods for the node for the OTS plan
--dv =============================================
CREATE PROCEDURE [dbo].[MID_OTS_PLAN_READ_METHODS_BY_NODE]
	@PLAN_HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ots.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, 
		au.USER_NAME
	FROM OTS_PLAN ots, 
		 METHOD m, 
		 APPLICATION_USER au
	WHERE ots.PLAN_HN_RID = @PLAN_HN_RID
		  and ots.METHOD_RID = m.METHOD_RID
		  and m.USER_RID = au.USER_RID

END
GO