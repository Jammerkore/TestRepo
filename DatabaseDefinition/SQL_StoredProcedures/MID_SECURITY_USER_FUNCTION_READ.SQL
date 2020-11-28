--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads security functions for users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_FUNCTION_READ]
	@FUNC_ID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSecurityActions_NotSpecified INT = 0;
	DECLARE @eSecurityLevel_NotSpecified INT = 0;

	SELECT 
		coalesce(ACTION_ID, @eSecurityActions_NotSpecified) ACTION_ID,
		coalesce(SEC_LVL_ID, @eSecurityLevel_NotSpecified) SEC_LVL_ID
	FROM SECURITY_USER_FUNCTION 
	WHERE USER_RID = @USER_RID
		  AND FUNC_ID = @FUNC_ID

END
GO
