--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads all merchandise security action text
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_MERCH_ACTIONS_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		sma.ACTION_ID, 
		at.TEXT_VALUE
	FROM SECURITY_MERCH_ACTIONS sma, 
		APPLICATION_TEXT at
	WHERE sma.ACTION_ID = at.TEXT_CODE
	ORDER BY sma.ACTION_ID

END
GO


