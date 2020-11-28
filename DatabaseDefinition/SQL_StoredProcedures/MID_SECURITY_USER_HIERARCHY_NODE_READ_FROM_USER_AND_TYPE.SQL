--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads security hierarchy nodes for the user and security type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_HIERARCHY_NODE_READ_FROM_USER_AND_TYPE]
	@USER_RID int,
	@SEC_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		hn.HN_RID HN_RID, 
		BN_ID, 
		ACTION_ID, 
		SEC_TYPE, 
		SEC_LVL_ID
	FROM SECURITY_USER_HIERARCHY_NODE SUHN, 
		 HIERARCHY_NODE hn, 
		 BASE_NODE bn
	WHERE SUHN.HN_RID = hn.HN_RID 
		  AND bn.HN_RID = hn.HN_RID 
		  AND USER_RID = @USER_RID
		  AND SEC_TYPE = @SEC_TYPE
	ORDER BY HOME_PH_RID, HOME_LEVEL

END
GO
