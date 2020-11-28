--dv =============================================
--dv Create date: 2/4/2014
--dv Description: Reads security user versions
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_VERSION_READ]
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
		coalesce(SEC_LVL_ID, @eSecurityLevel_NotSpecified) SEC_LVL_ID
	FROM SECURITY_USER_VERSION
	WHERE USER_RID = @USER_RID
		  AND FV_RID = @FV_RID
		  AND SEC_TYPE = @SEC_TYPE

END
GO
