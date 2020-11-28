--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads security functions for users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_FUNCTION_READ_FROM_USER]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		suf.FUNC_ID, 
		suf.ACTION_ID, 
		suf.SEC_LVL_ID, 
		at.TEXT_VALUE 
	FROM SECURITY_USER_FUNCTION suf, 
		 APPLICATION_TEXT at
	WHERE USER_RID = @USER_RID
		  AND suf.FUNC_ID = at.TEXT_CODE
	ORDER BY at.TEXT_VALUE

END
GO
