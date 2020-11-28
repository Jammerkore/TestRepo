--dv =============================================
--dv Create date:	11/18/2014
--dv Description:	Reads filters for the users passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_READ_FOR_USER]
	@FILTER_TYPE INT,
	@USER_RID_LIST USER_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID int = 4;
	
	SELECT 
		f.FILTER_RID, 
		f.USER_RID, 
		f.FILTER_NAME,
		f.OWNER_USER_RID, 
		f.USER_RID AS FILTER_USER_RID
	FROM FILTER f WITH (NOLOCK)
	INNER JOIN APPLICATION_USER au WITH (NOLOCK) on f.USER_RID = au.USER_RID
	WHERE f.FILTER_TYPE = @FILTER_TYPE
		  AND f.USER_RID IN (SELECT USER_RID FROM @USER_RID_LIST)
	ORDER BY f.FILTER_NAME

END
GO

