--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads a count of rows for the given user from the user option table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_OPTIONS_READ_COUNT]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM USER_OPTIONS 
	WHERE USER_RID = @USER_RID

END
GO

