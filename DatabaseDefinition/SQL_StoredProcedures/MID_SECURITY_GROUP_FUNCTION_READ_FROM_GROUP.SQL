--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads security group function assignments from the provided group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_FUNCTION_READ_FROM_GROUP]
	@GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		sgf.FUNC_ID, 
		sgf.ACTION_ID, 
		sgf.SEC_LVL_ID, 
		at.TEXT_VALUE
	FROM SECURITY_GROUP_FUNCTION sgf, 
		 APPLICATION_TEXT at
	WHERE sgf.GROUP_RID = @GROUP_RID
		  AND sgf.FUNC_ID = at.TEXT_CODE
	ORDER BY at.TEXT_VALUE

END
GO