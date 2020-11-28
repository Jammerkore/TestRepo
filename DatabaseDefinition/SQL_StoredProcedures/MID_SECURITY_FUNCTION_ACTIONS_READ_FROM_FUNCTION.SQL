--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads actions for the security function
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_FUNCTION_ACTIONS_READ_FROM_FUNCTION]
	@FUNC_ID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		sfa.FUNC_ID, 
		sfa.ACTION_ID, 
		at.TEXT_VALUE
	FROM SECURITY_FUNCTION_ACTIONS sfa, APPLICATION_TEXT at
	WHERE sfa.FUNC_ID = @FUNC_ID
		  AND sfa.ACTION_ID = at.TEXT_CODE
	ORDER BY sfa.ACTION_ID

END
GO
