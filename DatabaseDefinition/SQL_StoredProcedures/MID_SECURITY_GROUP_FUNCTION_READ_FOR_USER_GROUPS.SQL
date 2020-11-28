--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads security functions for user groups
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_FUNCTION_READ_FOR_USER_GROUPS]
	@FUNC_ID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSecurityActions_NotSpecified INT = 0;
	DECLARE @eSecurityLevel_NotSpecified INT = 0;

	SELECT 
		coalesce(ACTION_ID, @eSecurityActions_NotSpecified) ACTION_ID,
		coalesce(SEC_LVL_ID, @eSecurityLevel_NotSpecified) SEC_LVL_ID,
		sgf.GROUP_RID GROUP_RID
	FROM SECURITY_GROUP_FUNCTION sgf, 
		 USER_GROUP_JOIN ugj, 
		 USER_GROUP ug
	WHERE sgf.GROUP_RID = ugj.GROUP_RID 
		  AND ug.GROUP_RID = ugj.GROUP_RID 
		  AND ug.GROUP_ACTIVE_IND = '1'
		  AND sgf.FUNC_ID = @FUNC_ID
		  AND USER_RID = @USER_RID
	ORDER BY sgf.GROUP_RID

END
GO
