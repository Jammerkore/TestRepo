--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads all action text for versions
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_VERSION_ACTIONS_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		sva.ACTION_ID, 
		at.TEXT_VALUE
	FROM SECURITY_VERSION_ACTIONS sva, 
		 APPLICATION_TEXT at
	WHERE sva.ACTION_ID = at.TEXT_CODE
	ORDER BY sva.ACTION_ID

END
GO

