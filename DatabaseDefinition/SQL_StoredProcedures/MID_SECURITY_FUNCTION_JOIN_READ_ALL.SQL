--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads all security functions
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_FUNCTION_JOIN_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		suf.FUNC_ID, 
		suf.FUNC_TYPE, 
		suf.FUNC_LEVEL, 
		suf.FUNC_PARENT, 
		at.TEXT_VALUE
	FROM SECURITY_FUNCTION_JOIN suf, 
		 APPLICATION_TEXT at
	WHERE suf.FUNC_ID = at.TEXT_CODE
	ORDER BY suf.FUNC_LEVEL, suf.FUNC_PARENT, at.TEXT_VALUE

END
GO
