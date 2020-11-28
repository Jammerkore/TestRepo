--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads group assignment nodes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_HIERARCHY_NODE_READ_GROUP_ASSIGNMENT]
	@USER_RID int,
	@SEC_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSecurityActions_NotSpecified INT = 0;
	DECLARE @eDatabaseSecurityTypes_NotSpecified INT = 0;
	DECLARE @eSecurityLevel_NotSpecified INT = 0;

	SELECT 
		hn.HN_RID HN_RID, 
		BN_ID,
		coalesce(ACTION_ID, @eSecurityActions_NotSpecified) ACTION_ID,
		coalesce(SEC_TYPE, @eDatabaseSecurityTypes_NotSpecified) SEC_TYPE,
		coalesce(SEC_LVL_ID, @eSecurityLevel_NotSpecified) SEC_LVL_ID,
		sghn.GROUP_RID GROUP_RID
	FROM SECURITY_GROUP_HIERARCHY_NODE sghn, 
		 USER_GROUP_JOIN ugj, 
		 USER_GROUP ug, 
		 HIERARCHY_NODE hn, 
		 BASE_NODE bn
	WHERE sghn.GROUP_RID = ugj.GROUP_RID 
		  AND ug.GROUP_RID = ugj.GROUP_RID 
		  AND ug.GROUP_ACTIVE_IND = '1'
		  AND USER_RID = @USER_RID
		  AND sghn.HN_RID = hn.HN_RID 
		  AND bn.HN_RID = hn.HN_RID
		  AND SEC_TYPE = @SEC_TYPE
	ORDER BY HOME_PH_RID, HOME_LEVEL DESC, sghn.GROUP_RID

END
GO
