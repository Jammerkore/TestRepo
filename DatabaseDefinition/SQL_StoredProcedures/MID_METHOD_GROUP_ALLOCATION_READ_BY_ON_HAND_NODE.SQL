--dv =============================================
--dv Create date:	8/15/2014
--dv Description:	Reads group allocation methods based on the on hand node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GROUP_ALLOCATION_READ_BY_ON_HAND_NODE]
	@ON_HAND_HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mo.METHOD_RID, 
		m.METHOD_NAME, 
		m.METHOD_TYPE_ID,
		m.USER_RID, 
		au.USER_NAME
	FROM METHOD_GROUP_ALLOCATION mo
		 INNER JOIN METHOD m ON mo.METHOD_RID = m.METHOD_RID 
		 INNER JOIN APPLICATION_USER au ON m.USER_RID = au.USER_RID
	WHERE mo.ON_HAND_HN_RID = @ON_HAND_HN_RID
 


END
GO
