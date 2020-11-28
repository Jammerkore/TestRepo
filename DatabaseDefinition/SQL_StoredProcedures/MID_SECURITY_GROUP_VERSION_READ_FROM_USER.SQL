--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Reads security group versions
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_VERSION_READ_FROM_USER]
	@USER_RID int,
	@FV_RID int,
	@SEC_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSecurityActions_NotSpecified INT = 0;
	DECLARE @eDatabaseSecurityTypes_NotSpecified INT = 0;
	DECLARE @eSecurityLevel_NotSpecified INT = 0;

	SELECT 
		coalesce(ACTION_ID, @eSecurityActions_NotSpecified) ACTION_ID,
		coalesce(SEC_TYPE, @eDatabaseSecurityTypes_NotSpecified) SEC_TYPE,
		coalesce(SEC_LVL_ID, @eSecurityLevel_NotSpecified) SEC_LVL_ID,
		sgv.GROUP_RID GROUP_RID
	FROM SECURITY_GROUP_VERSION sgv, 
		 USER_GROUP_JOIN ugj, 
		 USER_GROUP ug
	WHERE sgv.GROUP_RID = ugj.GROUP_RID 
		  AND ug.GROUP_RID = ugj.GROUP_RID 
		  AND ug.GROUP_ACTIVE_IND = '1'
		  AND sgv.FV_RID = @FV_RID
		  AND USER_RID = @USER_RID
		  AND SEC_TYPE = @SEC_TYPE
	ORDER BY sgv.GROUP_RID

END
GO
